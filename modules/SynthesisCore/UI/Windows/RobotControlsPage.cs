using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;
using SynthesisCore.EntityControl;
using SynthesisCore.Simulation;
using System;
using System.Collections.Generic;

namespace SynthesisCore.UI.Windows
{
    public static class RobotControlsPage
    {
        public static VisualElement Page { get; private set; } = null;
        private static ListView controlList;
        private static Dropdown entityDropdown;
        private static Dropdown driveSchemeDropdown;

        private static CameraController.CameraState? cameraState = null;
        internal static Entity? selectedEntity { get; private set; } = null;

        private static Button setupControlsButton;
        private static List<VisualElement> driveSchemeSection = new List<VisualElement>();
        private static List<VisualElement> motorControlsSection = new List<VisualElement>();

        public static void Create(VisualElementAsset controlsAsset)
        {
            if (Page == null)
            {
                Page = controlsAsset.GetElement("page");
                Page.SetStyleProperty("height", "100%");

                controlList = (ListView)Page.Get("robot-controls");

                LoadPageContent();
            }
        }

        /// <summary>
        /// Get the entities with that have MotorAssemblyManager components, which mean they can be controlled by a RobotController
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Entity> controllableEntities()
        {
            var a = new Dictionary<string, Entity>();
            foreach(var i in SynthesisCoreData.ModelsDict)
            {
                if (i.Value.GetComponent<MotorAssemblyManager>() != null &&
                    i.Value.GetComponent<MotorAssemblyManager>().AllMotorAssemblies.Count > 0)
                {
                    a.Add(i.Key, i.Value);
                }
            }
            return a;
        }

        private static void LoadPageContent()
        {
            // Entity selector
            var entitySelector = Page.Get("entity-selector-dropdown-container");
            entityDropdown = new Dropdown("entity-dropdown", 0, controllableEntities().Keys);
            entityDropdown.ItemHeight = 15;
            entitySelector.Add(entityDropdown);

            selectedEntity = controllableEntities()[entityDropdown.Selected];
            setupControlsButton = (Button)Page.Get("setup-controls-button");

            entityDropdown.Subscribe(_ => {
                selectedEntity = controllableEntities()[entityDropdown.Selected]; // Select entity
                LookAtEntity();
                
                // Update the rest of the controls page based on the selected entity
                if (selectedEntity?.GetComponent<RobotController>() == null)
                {
                    RemoveEntityControlsSection();
                }
                else
                {
                    AddEntityControlsSection();
                    if(selectedEntity?.GetComponent<RobotController>().SelectedDriveScheme != null)
                    {
                        UpdateDriveSchemeSection();
                    }
                }
            });

            setupControlsButton.Subscribe(_ => {
                if (selectedEntity?.GetComponent<RobotController>() == null)
                {
                    selectedEntity?.AddComponent<RobotController>();
                    AddEntityControlsSection();
                }
                else
                {
                    selectedEntity?.RemoveComponent<RobotController>();
                    RemoveEntityControlsSection();
                }
            });
        }

        private static void AddEntityControlsSection()
        {
            setupControlsButton.Text = "Remove Controls";
            UpdateDriveSchemeSection();
        }

        private static void RemoveEntityControlsSection()
        {
            setupControlsButton.Text = "Setup Controls";
            ClearEntityControlsSection();
        }

        private static void ClearEntityControlsSection()
        {
            foreach (var i in driveSchemeSection)
            {
                i.RemoveFromHierarchy();
            }
            driveSchemeSection.Clear();
        }

        private static void UpdateDriveSchemeSection()
        {
            ClearEntityControlsSection();

            var divider = AddDivider();
            divider.Name = "drive-scheme-selector-divider";
            driveSchemeSection.Add(divider);

            var driveSchemeSelector = new VisualElement();
            driveSchemeSelector.Name = "drive-scheme-dropdown-container";
            driveSchemeSection.Add(AddRow("Select Drive Scheme", driveSchemeSelector));

            var driveSchemeOptions = new List<string>();
            foreach (var i in Enum.GetValues(typeof(RobotController.DriveScheme)))
            {
                driveSchemeOptions.Add(i.ToString());
            }

            driveSchemeDropdown = new Dropdown("drive-scheme-dropdown", 0, driveSchemeOptions);
            driveSchemeDropdown.ItemHeight = 15;
            driveSchemeSelector.Add(driveSchemeDropdown);

            var applyDriveSchemeButton = new Button();
            applyDriveSchemeButton.Name = "apply-drive-scheme-button";
            applyDriveSchemeButton.Text = "Apply";
            driveSchemeSection.Add(AddRow("", applyDriveSchemeButton));
            
            applyDriveSchemeButton.Subscribe(_ =>
            {
                applyDriveSchemeButton.Text = "Modify";
                var newDriveScheme = (RobotController.DriveScheme)Enum.Parse(typeof(RobotController.DriveScheme), driveSchemeDropdown.Selected);

                selectedEntity?.GetComponent<RobotController>().SetDriveScheme(newDriveScheme);

                if (selectedEntity?.GetComponent<RobotController>().SelectedDriveScheme != RobotController.DriveScheme.None)
                {
                    UIManager.ShowPanel("Configure Drivetrain Motors");
                }
                else
                {
                    UpdateMotorControlsSection();
                }
            });
            driveSchemeDropdown.Subscribe(_ =>
            {
                var newDriveScheme = (RobotController.DriveScheme)Enum.Parse(typeof(RobotController.DriveScheme), driveSchemeDropdown.Selected);
                var robotController = selectedEntity?.GetComponent<RobotController>();
                if (newDriveScheme != robotController?.SelectedDriveScheme)
                {
                    applyDriveSchemeButton.Text = "Apply";
                }
                else
                {
                    applyDriveSchemeButton.Text = "Modify";
                }
            });
        }

        private static void ClearMotorControlsSection()
        {
            foreach (var i in motorControlsSection)
            {
                i.RemoveFromHierarchy();
            }
            driveSchemeSection.RemoveAll(e => motorControlsSection.Contains(e));
            motorControlsSection.Clear();
        }

        internal static void UpdateMotorControlsSection()
        {
            ClearMotorControlsSection();

            var divider = AddDivider();
            divider.Name = "motor-controls-divider";
            motorControlsSection.Add(divider);

            switch (selectedEntity?.GetComponent<RobotController>().SelectedDriveScheme)
            {
                case RobotController.DriveScheme.Arcade:
                    // TODO
                    break;
                case RobotController.DriveScheme.Tank:
                    // TODO
                    break;
                case RobotController.DriveScheme.None:
                default:
                    break;
            }

            // TODO
            Logger.Log("TODO UpdateMotorControlsSection");

            driveSchemeSection.AddRange(motorControlsSection);
        }

        private static VisualElement AddRow(string labelText, VisualElement button)
        {
            var row = new VisualElement();
            row.AddToClassList("robot-control-row");
            StyleSheetManager.ApplyClassFromStyleSheets("robot-control-row", row);

            var label = new Label();
            label.Text = labelText;
            label.AddToClassList("robot-control-label");
            StyleSheetManager.ApplyClassFromStyleSheets("robot-control-label", label);

            if (!button.ClassesContains("robot-control-button"))
            {
                button.AddToClassList("robot-control-button");
                StyleSheetManager.ApplyClassFromStyleSheets("robot-control-button", button);
            }

            row.Add(label);
            row.Add(button);

            controlList.Add(row);

            return row;
        }

        private static VisualElement AddDivider()
        {
            var divider = new VisualElement();
            divider.AddToClassList("vertical-divider");
            StyleSheetManager.ApplyClassFromStyleSheets("vertical-divider", divider);

            controlList.Add(divider);
            
            return divider;
        }

        internal static void LookAtEntity()
        {
            if (cameraState == null)
            {
                cameraState = CameraController.CameraState.Save();
            }
            var pos = selectedEntity?.GetComponent<Transform>()?.GlobalPosition;
            if (pos.HasValue)
                CameraController.Instance.SetNewFocus(pos.Value, true);
        }

        internal static void StopLookAtEntity()
        {
            if (cameraState != null)
            {
                CameraController.CameraState.Restore(cameraState.Value);
                cameraState = null;
            }
        }
    }
}