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
            Entity cube = SimulationManager.SpawnEntity();

            cube.GetComponent<Transform>().Position = new Vector3D(0, 5, 5);
            Mesh m = cube.GetComponent<Mesh>();
            Cube.Make(m);
            SynthesisCoreData.ModelsDict.Add("Cube", cube);
            
            // cube.AddComponent<Moveable>().Channel = 5;

            GltfAsset g = AssetManager.GetAsset<GltfAsset>("/modules/synthesis_core/Test.glb");
            var testBody = SimulationManager.SpawnEntity(g);

            SynthesisCoreData.ModelsDict.Add("TestBody", testBody);

            testBody.AddComponent<Moveable>().Channel = 5;

            foreach (var i in EnvironmentManager.GetComponentsWhere<Rigidbody>(_ => true))
            {
                i.AngularDrag = 0;
            }
        }

        public override void OnUpdate() { }

        public override void Teardown() { }

        public override void OnPhysicsUpdate() { }
    }
}
