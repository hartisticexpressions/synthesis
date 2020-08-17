using MathNet.Spatial.Euclidean;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EventBus;
using SynthesisAPI.Modules.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConvexLibraryWrapper;
using MIConvexHull;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.Utilities;
using VHACD;
using ConvexHull = ConvexLibraryWrapper.ConvexHull;

namespace SynthesisCore.Systems
{
    [ModuleExport]
    class Descomposer : SystemBase
    {
        private double startLen = 0;
        private double progress = 0;
        private double processed = 0;
        private double tick = 0;
        List<MeshDecompEvent> queue = new List<MeshDecompEvent>();
        public override void OnPhysicsUpdate()
        {
        }

        public override void Teardown()
        {
           
        }

        public override void OnUpdate()
        {
            
            
            //Logger.Log("This is a G rated test @hunter", LogLevel.Debug);
            if (queue.Count > 0)
            {
                if (startLen <= 0)
                {
                    startLen = queue.Count;
                }    
                
                progress = processed / queue.Count;
                ProcessRequest(queue[0]);
                processed++;
                queue.RemoveAt(0);
                if (tick > 60 * 3)
                {
                    Logger.Log("Collision "+((int)(progress * 100))+"% Processed (" + processed + "/"  + queue.Count + ")", LogLevel.Debug);
                    tick = 0;
                }

                tick++;
                // 
            }
            else
            {
                startLen = 0;
                processed = 0;
                processed = 0;
                tick = 0;
                
            }

          
        }

        public override void Setup()
        {
            
        }

        [TaggedCallback("ProcessCollision")]
        public void MeshDecompHandler(MeshDecompEvent e)
        {
            if (e.inputMesh == null)
            {
                Logger.Log("No Mesh Mate tf u doin", LogLevel.Error);
                return;
            }
            queue.Add(e);
          
        }
        
        [TaggedCallback("ProcessCollisionFinished")]
        public void FinishedHandler(MeshDecompFinishedEvent evt)
        {
            //Logger.Log("finished! lol uWu much decompose such wow");
            Entity e = EnvironmentManager.AddEntity();
          
            e.AddComponent<Transform>().Position = new Vector3D(0,0,20);
            Mesh m = e.AddComponent<Mesh>();
            m.Triangles = evt.resultMesh.Triangles;
            m.Vertices = evt.resultMesh.Vertices;
            m.UVs = evt.resultMesh.UVs;
        }

        private void ProcessRequest(MeshDecompEvent comp)
        {
            
            
            
            IVHACD ivhacd = new IVHACD();
            
            
            Parameters parameters = new Parameters();
            parameters.m_resolution = 1000;
            parameters.m_convexhullDownsampling = 5;
            parameters.m_concavity = 1.0;
            
            
           
            if (ivhacd.Compute(
                CollisionUtils.Cvt_MeshToVertArray(comp.inputMesh).ToArray(),
                3,
                (uint) comp.inputMesh.Vertices.Count ,
                comp.inputMesh.Triangles.ToArray(),
                3,
                (uint) comp.inputMesh.Triangles.Count,
                parameters
                ))
            {
                MeshDecompFinishedEvent evt = new MeshDecompFinishedEvent();
                ConvexHull ch;
                uint nConvexHulls = ivhacd.GetNConvexHulls();
                Mesh m = new Mesh();
                uint vOffset = 0;
                for (uint x = 0; x < nConvexHulls; x++)
                {
                    ch = ivhacd.GetConvexHull(x);
                    for (uint i = 0; i < ch.m_nPoints * 3; i += 3)
                    {
                        m.Vertices.Add(new Vector3D(ch.m_points[i], ch.m_points[1+i], ch.m_points[i+2]));   
                    }
                    
                    for (uint i = 0; i < ch.m_nTriangles * 3; i+=3)
                    {
                        m.Triangles.Add((int) (ch.m_triangles[i] + vOffset - 1));
                        m.Triangles.Add((int) (ch.m_triangles[i + 1] + vOffset - 1));
                        m.Triangles.Add((int) (ch.m_triangles[i + 2] + vOffset - 1));
                    }

                    vOffset += ch.m_nPoints;

                }
                ivhacd.Clean();
                evt.resultMesh = m;
                EventBus.Push("ProcessCollisionFinished",evt);
            }
            else
            {
                Logger.Log("VHACD Failed", LogLevel.Error);
            }
            
           
        }

    }
}
