using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisCore.Simulation;
using System.Linq;

namespace SynthesisCore.EntityControl
{
    public static class SimulationManager
    {
        public static Entity? ActiveEnvironment { get; private set; } = null;

        public static Entity SpawnEntity(GltfAsset modelAsset = null)
        {
            var entity = EnvironmentManager.AddEntity();

            if (modelAsset != null)
            {
                entity.AddBundle(modelAsset?.Parse());
            }
            else
            {
                entity.AddComponent<Transform>();
                entity.AddComponent<Mesh>();
                entity.AddComponent<MeshCollider>();
                entity.AddComponent<Rigidbody>();
            }

            entity.AddComponent<Selectable>();

            if (entity.GetComponent<Joints>() != null &&
                entity.GetComponent<Joints>().AllJoints.Count > 0 &&
                entity.GetComponent<Joints>().AllJoints.Any(j => j is HingeJoint)
            )
            {
                var motorAssemblyManager = entity.AddComponent<MotorAssemblyManager>();
                foreach (var motorAssembly in motorAssemblyManager.AllMotorAssemblies)
                {
                    motorAssembly.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29); // Default configuration
                }
            }

            return entity;
        }

        public static void ConfigureEntity()
        {
            // TODO setup joints and robot controls
        }

        public static void ChangeEnvironment()
        {
            // TODO
        }
    }
}
