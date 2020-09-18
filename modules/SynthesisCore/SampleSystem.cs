using MathNet.Spatial.Euclidean;
using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisCore.Components;
using SynthesisCore.Meshes;
using SynthesisCore.EntityControl;
using SynthesisAPI.VirtualFileSystem;
using SynthesisAPI.InputManager;
using SynthesisAPI.Utilities;

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

            //GltfAsset glb = AssetManager.GetAsset<GltfAsset>("/modules/synthesis_core/Test.glb");
            //Bundle b = glb.Bundle;

            //Entity e = EnvironmentManager.AddEntity();
            //InputManager.AssignDigitalInput("add", new SynthesisAPI.InputManager.Inputs.Digital("i"), (_) =>
            //{
            //    if (e.RemoveEntity())
            //    {
            //        Entity entity = EnvironmentManager.AddEntity();
            //        entity.AddBundle(b);
            //        Logger.Log("test");
            //    }
            //});

            //e.AddBundle(b);

            // ConfigManager.Test("test.json");
            EntityImportManager.RefreshModelList();
        }

        public override void OnUpdate() { }

        public override void Teardown() { }

        public override void OnPhysicsUpdate() { }
    }
}
