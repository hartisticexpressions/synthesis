using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using System.Collections.Generic;

namespace SynthesisCore.Simulation
{
    /// <summary>
    /// Manages all of the motor assemblies for an entity and its descendants
    /// </summary>
    public class MotorAssemblyManager : Component
    {
        public void AddMotor(MotorAssembly controller)
        {
            AllMotorAssemblies.Add(controller);
        }

        public void RemoveMotor(int index)
        {
            AllMotorAssemblies.RemoveAt(index);
        }

        private List<MotorAssembly> _allMotorAssemblies = null;

        public List<MotorAssembly> AllMotorAssemblies
        {
            get
            {
                if (_allMotorAssemblies == null)
                {
                    _allMotorAssemblies = Setup(Entity.Value);
                }
                return _allMotorAssemblies;
            }
            set => _allMotorAssemblies = value;
        }

        private static List<MotorAssembly> Setup(Entity thisEntity)
        {
            var allMotorAssemblies = new List<MotorAssembly>();

            foreach (var entity in EnvironmentManager.GetEntitiesWhere(
                e => Parent.IsDescendant(thisEntity, e) &&
                e.GetComponent<Joints>() != null))
            {
                foreach (var j in entity.GetComponent<Joints>().AllJoints)
                {
                    if (j is HingeJoint hingeJoint)
                    {
                        allMotorAssemblies.Add(new MotorAssembly(entity, hingeJoint));
                    }
                }
            }
            return allMotorAssemblies;
        }

        public MotorAssemblyManager()
        {
            /*
            Joints.GlobalAddJoint += j =>
            {
                if (j is HingeJoint hingeJoint)
                {
                    AllGearBoxes.Add(new MotorAssembly(hingeJoint, MotorFactory.CIMMotor(), 9.29, 1));
                }
            };
            */
        }
    }
}