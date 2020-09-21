using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;
using SynthesisCore.EntityControl;
using System;
using System.Collections.Generic;

namespace SynthesisCore.UI.Windows
{
    public class RobotControlsPage
    {
        public VisualElement Page { get; private set; } = null;
        private static ListView controlListView;
        private static Dropdown entityDropdown;
        private static Dropdown driveSchemeDropdown;

        private static CameraController.CameraState? cameraState = null;
        internal static Entity? selectedEntity { get; private set; } = null;

        private static Button setupControlsButton;
        private static List<VisualElement> driveSchemeSection = new List<VisualElement>();
        private static List<VisualElement> motorControlsSection = new List<VisualElement>();

        private static VisualElementAsset controlAsset;
        private static List<ControlItem> controlsList = new List<ControlItem>();

        private static VisualElement entitySelector;
        public RobotControlsPage(VisualElementAsset robotControlsAsset)
        {
            Page = robotControlsAsset.GetElement("page");
            Page.SetStyleProperty("height", "100%");

            controlListView = (ListView)Page.Get("robot-controls");
            controlAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Control.uxml");

            LoadPageContent();
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
                // for debugging since sample models do not have motors
                //if (i.Entity?.GetComponent<MotorAssemblyManager>() != null &&
                //    i.Entity?.GetComponent<MotorAssemblyManager>().AllMotorAssemblies.Count > 0)
                //{
                    a.Add(i.Value, i.Entity.Value);
                //}
            }
            return a;
        }

        private void LoadPageContent()
        {
            // Entity selector
            setupControlsButton = (Button)Page.Get("setup-controls-button");
            entitySelector = Page.Get("entity-selector-dropdown-container");
            entityDropdown = new Dropdown("entity-dropdown", 0, controllableEntities().Keys);
            entityDropdown.ItemHeight = 15;
            entitySelector.Add(entityDropdown);

            UpdateDropdownSelectedEntity();

            entityDropdown.Subscribe(_ => {
                UpdateDropdownSelectedEntity();
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

        private static void UpdateDropdownSelectedEntity()
        {
            selectedEntity = entityDropdown.Selected != null ? (Entity?)controllableEntities()[entityDropdown.Selected] : null;
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

            var robotController = selectedEntity?.GetComponent<RobotController>();

            // TODO try load from preferences? Will need to save RobotController configuration to preferences as well
            robotController.ResetControls();

            foreach(var (controlName, _) in robotController.Inputs)
            {
                AddControl(controlName);
            }

            driveSchemeSection.AddRange(motorControlsSection);
        }

        private static void AddControl(string controlName)
        {
            var item = new ControlItem(controlAsset, controlName);
            controlsList.Add(item);
            motorControlsSection.Add(item.Element);
            controlListView.Add(item.Element);
        }

        public static void RefreshPreferences()
        {
            controlsList.ForEach(control => control.UpdateInformation());
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

            controlListView.Add(row);

            return row;
        }

        private static VisualElement AddDivider()
        {
            var divider = new VisualElement();
            divider.AddToClassList("vertical-divider");
            StyleSheetManager.ApplyClassFromStyleSheets("vertical-divider", divider);

            controlListView.Add(divider);
            
            return divider;
        }

        internal void LookAtEntity()
        {
            if (cameraState == null)
            {
                cameraState = CameraController.CameraState.Save();
            }
            var pos = selectedEntity?.GetComponent<Transform>()?.GlobalPosition;
            if (pos.HasValue)
                CameraController.Instance.SetNewFocus(pos.Value, true);
        }

        internal void StopLookAtEntity()
        {
            if (cameraState != null)
            {
                CameraController.CameraState.Restore(cameraState.Value);
                cameraState = null;
            }
        }
    }
}