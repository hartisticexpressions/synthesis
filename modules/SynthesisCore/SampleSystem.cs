using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using SynthesisAPI.AssetManager;
using SynthesisAPI.DevelopmentTools;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.EventBus;
using SynthesisAPI.Modules.Attributes;
using SynthesisAPI.Utilities;
using SynthesisCore.Components;
using SynthesisCore.Systems;
using SynthesisAPI.InputManager;
using SynthesisAPI.InputManager.Inputs;
using SynthesisCore.Systems;

namespace SynthesisCore
{
    public class SampleSystem : SystemBase
    {
        private Entity testBody;

        private MotorController frontLeft, frontRight, backLeft, backRight;
        private MotorController arm;

        public Mesh m;

        public override void OnPhysicsUpdate() { }

        public override void Setup()
        {
            testBody = EnvironmentManager.AddEntity();
            
            Entity e = EnvironmentManager.AddEntity();
            
            
            GltfAsset g = AssetManager.GetAsset<GltfAsset>("/modules/synthesis_core/Test.glb");
            Bundle o = g.Parse();
            testBody.AddBundle(o);
            e.AddBundle(o);
            List<Mesh> meshes = CollisionUtils.Cvt_BundleToMesh(o);
            
            foreach (Mesh mesh in meshes)
            {
                MeshDecompEvent evt = new MeshDecompEvent();
                evt.inputMesh = mesh;
                EventBus.Push("ProcessCollision", evt);
            }
            





            // e.AddComponent<Transform>();
            // e.AddComponent<Selectable>();
            // e.AddComponent<Moveable>().Channel = 5;
            // Mesh m = e.AddComponent<Mesh>();

        }

        public override void OnUpdate() {
            float forward = InputManager.GetAxisValue("vert");
            float turn = InputManager.GetAxisValue("hori");
            
            var moveArmForward = new Digital("t");
            moveArmForward.Update();
            if (moveArmForward.State == DigitalState.Down) {
                arm.SetPercent(0.5f);
            } else {
                arm.SetPercent(0.0f);
            }

            frontLeft.SetPercent(forward + turn);
            frontRight.SetPercent(forward - turn);
            backLeft.SetPercent(forward + turn);
            backRight.SetPercent(forward - turn);
        }

        private Mesh cube()
        {
            if (m == null)
                m = new Mesh();

            m.Vertices = new List<Vector3D>()
            {
                new Vector3D(-0.5,-0.5,-0.5),
                new Vector3D(0.5,-0.5,-0.5),
                new Vector3D(0.5,0.5,-0.5),
                new Vector3D(-0.5,0.5,-0.5),
                new Vector3D(-0.5,0.5,0.5),
                new Vector3D(0.5,0.5,0.5),
                new Vector3D(0.5,-0.5,0.5),
                new Vector3D(-0.5,-0.5,0.5)
            };
            m.Triangles = new List<int>()
            {
                0, 2, 1, //face front
			    0, 3, 2,
                2, 3, 4, //face top
			    2, 4, 5,
                1, 2, 5, //face right
			    1, 5, 6,
                0, 7, 4, //face left
			    0, 4, 3,
                5, 4, 7, //face back
			    5, 7, 6,
                0, 6, 7, //face bottom
			    0, 1, 6
            };
            return m;
        }

        public override void Teardown() { }

        /*
        [TaggedCallback("input/move")]
        public void Move(DigitalEvent digitalEvent)
        {
            if(digitalEvent.State == DigitalState.Held)
            {
                switch (digitalEvent.Name)
                {
                    case "w":
                        ApiProvider.Log("w");
                        break;
                    case "a":
                        ApiProvider.Log("a");
                        break;
                    case "s":
                        ApiProvider.Log("s");
                        break;
                    case "d":
                        ApiProvider.Log("d");
                        break;
                    default:
                        break;
                }
            }
        }
        */
    }
}
