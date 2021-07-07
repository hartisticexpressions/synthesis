using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

using UMaterial = UnityEngine.Material;

namespace SynthesisAPI.Proto {
    
    /// <summary>
    /// Partial class to add utility functions and properties to Protobuf types
    /// </summary>
    public partial class UniqueMesh {
        public (Mesh unityMesh, List<UMaterial> materials) CreateMesh(SimObject source) {
            Mesh m = new Mesh();
            m.indexFormat = IndexFormat.UInt32;
            List<Vector3> verts = new List<Vector3>();
            int[] tris = new int[Triangles.Count];
            SubMeshDescriptor[] descriptors = new SubMeshDescriptor[SubMeshes.Count];
            for (int j = 0; j < Vertices.Count; j++) {
                verts.Add(Vertices[j]);
            }
            for (int i = 0; i < Triangles.Count; i++) {
                tris[i] = Triangles[i];
            }
            for (int i = 0; i < SubMeshes.Count; i++) {
                descriptors[i] = new SubMeshDescriptor(SubMeshes[i].Start, SubMeshes[i].Count);
            }
            m.vertices = verts.ToArray();
            m.triangles = tris;
            m.SetSubMeshes(descriptors);
            m.RecalculateNormals();

            var mats = new List<UMaterial>();
            for (int i = 0; i < SubMeshes.Count; i++) {
                // This really isn't necessary, I just got bored
                mats.Add(source.ParseMaterials().ParsedMaterials[SubMeshes[i].Material]);
            }

            // Debug.Log($"{verts.Count} -> {m.vertices.Length}");
            // Debug.Log($"{tris.Length} -> {m.triangles.Length}");
            // Debug.Log($"Polygons: {m.triangles.Length / 3}");
            
            return (m, mats);
        }
    }
}
