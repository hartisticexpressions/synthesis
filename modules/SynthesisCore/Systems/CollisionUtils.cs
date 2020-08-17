using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Spatial.Euclidean;
using MIConvexHull;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.Utilities;

namespace SynthesisCore.Systems
{
    public class CollisionUtils
    {
        public static List<float> Cvt_MeshToVertArray(Mesh mesh)
        {
            List<float> flatVert = new List<float>();
            foreach (var vert in mesh.Vertices)
            {
                flatVert.Add((float) vert.X);
                flatVert.Add((float) vert.Y);
                flatVert.Add((float) vert.Z);
            }

            return flatVert;
        }
       
        public static Mesh Cvt_HullToMesh(ConvexHull<DefaultVertex, DefaultConvexFace<DefaultVertex>> hull)
        {
            Mesh mesh = new Mesh();
            
            int tri = 0;
            foreach (DefaultConvexFace<DefaultVertex> face in hull.Faces)
            {    
               
                foreach (DefaultVertex defaultVertex in face.Vertices)
                {
                    mesh.Vertices.Add(new Vector3D(defaultVertex.Position[0], defaultVertex.Position[1], defaultVertex.Position[2]));
                    mesh.Triangles.Add(tri);
                    
                    tri++;
                }
                //
                
                
            }
            

            return mesh;
        }
        

        public static List<Mesh> Cvt_BundleToMesh(Bundle bundle)
        {
            List<Mesh> meshes = new List<Mesh>();
            int meshAdded = 0;
            
            foreach (Component component in bundle.Components)
            {
                if (component.GetType() == typeof(Mesh)) 
                {
                    meshes.Add((Mesh)component);
                    //Logger.Log("Adding Mesh: " +component.ToString());
                }
            }
            
            foreach (Bundle childBundle in bundle.ChildBundles)
            {
                meshes.AddRange(Cvt_BundleToMesh(childBundle));
            }

            return meshes;
        } 
    }
}