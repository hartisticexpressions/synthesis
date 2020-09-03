using MathNet.Spatial.Euclidean;
using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisCore.Components;
using SynthesisCore.Simulation;
using SynthesisCore.Meshes;

namespace SynthesisCore
{
    public class SampleSystem : SystemBase
    {
        private Entity testBody;

        private MotorAssemblyManager motorManager;
        private MotorAssembly frontLeft, frontRight, backLeft, backRight, arm;
        
        public override void Setup()
        {
            Entity cube = EnvironmentManager.AddEntity();

            cube.AddComponent<Transform>().Position = new Vector3D(0, 5, 5);
            Mesh m = cube.AddComponent<Mesh>();
            Cube.Make(m);
            cube.AddComponent<MeshCollider>();
            cube.AddComponent<Rigidbody>();
            var cubeSelectable = cube.AddComponent<Selectable>();
            //e.AddComponent<Moveable>().Channel = 5;
            SynthesisCoreData.ModelsDict.Add("Cube", cube);

            testBody = EnvironmentManager.AddEntity();

            GltfAsset g = AssetManager.GetAsset<GltfAsset>("/modules/synthesis_core/Test.glb");
            Bundle o = g.Parse();
            testBody.AddBundle(o);
            SynthesisCoreData.ModelsDict.Add("TestBody", testBody);
            var body = testBody.GetComponent<Joints>().AllJoints;

            var selectable = testBody.AddComponent<Selectable>();

            testBody.AddComponent<Moveable>().Channel = 5;

            motorManager = testBody.AddComponent<MotorAssemblyManager>();

            frontLeft = motorManager.AllMotorAssemblies[3];
            frontRight = motorManager.AllMotorAssemblies[4];
            backLeft = motorManager.AllMotorAssemblies[1];
            backRight = motorManager.AllMotorAssemblies[2];
            arm = motorManager.AllMotorAssemblies[0];

            frontLeft.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29);
            frontRight.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29);
            backLeft.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29);
            backRight.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29);
            arm.Configure(MotorTypes.Get("CIM"), gearReduction: 9.29);

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
