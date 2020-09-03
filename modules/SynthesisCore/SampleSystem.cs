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
            
            // cube.AddComponent<Moveable>().Channel = 5;

            var testBody = SimulationManager.SpawnEntity(
                AssetManager.GetAsset<GltfAsset>("/modules/synthesis_core/Test.glb"),
                "TestBody");

            testBody.AddComponent<Moveable>().Channel = 5;
        }

        public override void OnUpdate() { }

        public override void Teardown() { }

        public override void OnPhysicsUpdate() { }
    }
}
