using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EventBus;
using SynthesisAPI.InputManager;
using SynthesisAPI.InputManager.InputEvents;
using SynthesisAPI.InputManager.Inputs;
using SynthesisAPI.PreferenceManager;
using SynthesisCore.Simulation;
using System.Collections.Generic;

namespace SynthesisCore.EntityControl
{
    public class RobotController : Component
    {
        public enum DriveScheme
        {
            None,
            Arcade,
            Tank
        }

        public List<MotorAssembly> leftDrivetrainMotors;
        public List<MotorAssembly> rightDrivetrainMotors;
        public List<MotorAssembly> otherMotors;

        public DriveScheme? SelectedDriveScheme { get; private set; }

        public List<(string, Digital)> Inputs { get; private set; }

        public RobotController()
        {
            leftDrivetrainMotors = new List<MotorAssembly>();
            rightDrivetrainMotors = new List<MotorAssembly>();
            otherMotors = new List<MotorAssembly>();

            SelectedDriveScheme = null;

            Inputs = new List<(string, Digital)>();
        }

        public void SetDriveScheme(DriveScheme driveScheme)
        {
            if (SelectedDriveScheme != driveScheme)
            {
                SelectedDriveScheme = driveScheme;
            }
        }

        private void SetDrivetrainVoltages(IEvent e, double? leftVoltagePercent = null, double? rightVoltagePercent = null)
        {
            if (leftVoltagePercent.HasValue)
            {
                foreach (var i in leftDrivetrainMotors)
                {
                    SetVoltageCallback(e, i, leftVoltagePercent.Value);
                }
            }
            if (rightVoltagePercent.HasValue)
            {
                foreach (var i in rightDrivetrainMotors)
                {
                    SetVoltageCallback(e, i, rightVoltagePercent.Value);
                }
            }
        }

        private void SetVoltageCallback(IEvent e, MotorAssembly motorAssembly, double voltagePercent)
        {
            if(e is DigitalEvent digital)
            {
                if(digital.State == DigitalState.Down || digital.State == DigitalState.Held)
                {
                    motorAssembly.SetVoltage(
                        SynthesisAPI.Utilities.Math.Clamp(motorAssembly.Voltage + PowerSupply.Default12V.VoltagePercent(voltagePercent), 
                        -PowerSupply.Default12V.Voltage, 
                        PowerSupply.Default12V.Voltage));
                }
                else if(digital.State == DigitalState.Up)
                {
                    motorAssembly.SetVoltage(0);
                }
            }
        }

        private void AddControl(string controlName, string keyName, EventBus.EventCallback callback)
        {
            controlName = $"Entity {Entity.Value.Index} {controlName}";
            var key = new Digital(keyName);
            Inputs.Add((controlName, key));
            InputManager.AssignDigitalInput(controlName, key, callback);
        }

        public void ResetControls()
        {
            foreach (var (controlName, _) in Inputs)
            {
                InputManager.UnassignDigitalInput(controlName);
            }
            Inputs.Clear();

            switch (SelectedDriveScheme)
            {
                case DriveScheme.Arcade:
                    {
                        AddControl("Drivetrain forward", "w", e => SetDrivetrainVoltages(e, 1, 1));
                        AddControl("Drivetrain backward", "s", e => SetDrivetrainVoltages(e, -1, -1));
                        AddControl("Drivetrain rotate left", "a", e => SetDrivetrainVoltages(e, -1, 1));
                        AddControl("Drivetrain rotate right", "d", e => SetDrivetrainVoltages(e, 1, -1));
                        break;
                    }
                case DriveScheme.Tank:
                    {
                        AddControl("Drivetrain left forward", "w", e => SetDrivetrainVoltages(e, leftVoltagePercent: 1));
                        AddControl("Drivetrain left backward", "s", e => SetDrivetrainVoltages(e, leftVoltagePercent: -1));
                        AddControl("Drivetrain right forward", "e", e => SetDrivetrainVoltages(e, rightVoltagePercent: 1));
                        AddControl("Drivetrain right backward", "d", e => SetDrivetrainVoltages(e, rightVoltagePercent: -1));
                        break;
                    }
                case DriveScheme.None:
                default:
                    break;
            }


            for (var i = 0 ; i < otherMotors.Count; i++)
            {
                int iCopy = i; // Create new object so callback won't reference i
                var keyNum = SynthesisAPI.Utilities.Math.Min(i, 9).ToString();
                AddControl($"Motor {i} forward", keyNum, e => SetVoltageCallback(e, otherMotors[iCopy], 1));
                AddControl($"Motor {i} backward", $"[{keyNum}]", e => SetVoltageCallback(e, otherMotors[iCopy], -1));
            }

            foreach (var (option, key) in Inputs)
            {
                if (!PreferenceManager.ContainsPreference("SynthesisCore", option))
                {
                    PreferenceManager.SetPreference("SynthesisCore", option, key.Name);
                }
            }
            PreferenceManager.Save();

        }
    }
}
