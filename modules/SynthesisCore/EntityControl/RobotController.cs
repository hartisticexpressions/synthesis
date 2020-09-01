using SynthesisAPI.EnvironmentManager;
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

        public RobotController()
        {
            leftDrivetrainMotors = new List<MotorAssembly>();
            rightDrivetrainMotors = new List<MotorAssembly>();
            otherMotors = new List<MotorAssembly>();

            SelectedDriveScheme = null;
        }

        public void SetDriveScheme(DriveScheme driveScheme)
        {
            if (SelectedDriveScheme != driveScheme)
            {
                SelectedDriveScheme = driveScheme;
            }
        }
    }
}
