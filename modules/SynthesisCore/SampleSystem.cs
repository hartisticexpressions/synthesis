using MathNet.Spatial.Euclidean;
using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisCore.Components;
using SynthesisCore.Meshes;
using SynthesisCore.EntityControl;

namespace SynthesisCore
{
    public class SampleSystem : SystemBase
    {
        public override void Setup()
        {
            Entity cube = SimulationManager.SpawnEntity(name: "Cube");

            cube.GetComponent<Transform>().Position = new Vector3D(0, 5, 5);
            Mesh m = cube.GetComponent<Mesh>();
            Cube.Make(m);

            // ConfigManager.Test("test.json");
            EntityImportManager.RefreshModelList();
        }

        public override void OnUpdate() { }

        public override void Teardown() { }

        public override void OnPhysicsUpdate() { }
    }
}
