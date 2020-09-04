using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisCore.EntityControl;
using SynthesisCore.Simulation;
using System.Collections.Generic;

namespace SynthesisCore.UI.Windows
{
    public class DrivetrainConfigurationWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;
        private Dropdown motorTypeDropdown;
        private Label motorTypeLabel;

        private int motorIndex = 0;
        private List<MotorAssembly>? motorAssemblies => RobotControlsPage.selectedEntity?.GetComponent<MotorAssemblyManager>()?.AllMotorAssemblies;

        public readonly List<MotorAssembly> pendingLeftDrivetrainMotors;
        public readonly List<MotorAssembly> pendingRightDrivetrainMotors;
        public readonly List<MotorAssembly> pendingOtherMotors;

        public DrivetrainConfigurationWindow()
        {
            var drivetrainConfigurationAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/DrivetrainConfiguration.uxml");
            Panel = new Panel("Configure Drivetrain Motors", drivetrainConfigurationAsset, OnWindowOpen, false);

            pendingLeftDrivetrainMotors = new List<MotorAssembly>();
            pendingRightDrivetrainMotors = new List<MotorAssembly>();
            pendingOtherMotors = new List<MotorAssembly>();
        }

        private void OnWindowOpen(VisualElement window)
        {
            Window = window;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;

            motorIndex = 0;
            HighlightNextMotor();

            LoadWindowContents();
            RegisterButtons();
        }

        private void LoadWindowContents()
        {
            motorTypeLabel = (Label)Window.Get("motor-type-configure-label");
            UpdateMotorTypeLabel();

            var dropdownContainer = Window.Get("motor-type-dropdown-container");

            motorTypeDropdown = new Dropdown("motor-type-dropdown", 0, new string[] { "Other", "Left drivetrain motor", "Right drivetrain motor", "Ignore" });
            motorTypeDropdown.ItemHeight = 25;
            dropdownContainer.Add(motorTypeDropdown);
        }

        private void RegisterButtons()
        {
            Button nextButton = (Button)Window.Get("next-button");
            nextButton?.Subscribe(_ => {
                switch (motorTypeDropdown.Selected)
                {
                    case "Left drivetrain motor":
                        pendingLeftDrivetrainMotors.Add(motorAssemblies[motorIndex]);
                        break;
                    case "Right drivetrain motor":
                        pendingRightDrivetrainMotors.Add(motorAssemblies[motorIndex]);
                        break;
                    case "Ignore":
                        break;
                    case "Other":
                    default:
                        pendingOtherMotors.Add(motorAssemblies[motorIndex]);
                        break;
                }

                motorIndex++;

                if(motorIndex == motorAssemblies.Count)
                {
                    var robotController = RobotControlsPage.selectedEntity?.GetComponent<RobotController>();
                    robotController.leftDrivetrainMotors = pendingLeftDrivetrainMotors;
                    robotController.rightDrivetrainMotors = pendingRightDrivetrainMotors;
                    robotController.otherMotors = pendingOtherMotors;
                    
                    Highlighter.UnhighlightJoint();
                    UIManager.ClosePanel("Configure Drivetrain Motors");
                    RobotControlsPage.UpdateMotorControlsSection();
                }
                else
                {
                    HighlightNextMotor();
                    UpdateMotorTypeLabel();
                }
            });

            Button cancelButton = (Button)Window.Get("cancel-button");
            cancelButton?.Subscribe(_ => {
                Highlighter.UnhighlightJoint();
                UIManager.ClosePanel("Configure Drivetrain Motors");
            });
        }

        private void UpdateMotorTypeLabel()
        {
            motorTypeLabel.Text = $"Select type of highlighted motor {motorIndex + 1}/{motorAssemblies.Count}";
        }

        private void HighlightNextMotor()
        {
            var motorAssembly = motorAssemblies[motorIndex];
            Highlighter.HighlightJoint(motorAssembly.Joint, motorAssembly.Entity);

        }
    }
}