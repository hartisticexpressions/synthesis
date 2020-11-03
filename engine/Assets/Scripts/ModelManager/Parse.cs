using System;
using SharpGLTF.Schema2;
using SharpGLTF.IO;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.ModelManager;

namespace Synthesis.ModelManager
{
    public static class Parse
    {
        //for testing call -> Synthesis.ModelManager.Parse.AsRobot("Assets/Testing/Test.glb");

        private static bool tryFix = true;

        public static GameObject AsRobot(string filePath)
        {
            ModelRoot model = GetModelInfo(filePath);
            GameObject gameObject = CreateGameObject(model.DefaultScene.VisualChildren.First());
            return gameObject;
        }

        public static GameObject AsField(string filePath)
        {
            return null;
        }

        private static ModelRoot GetModelInfo(string filePath)
        {
            //set up file
            byte[] data = File.ReadAllBytes(filePath);
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            //read glb
            
            var settings = tryFix ? SharpGLTF.Validation.ValidationMode.TryFix : SharpGLTF.Validation.ValidationMode.Strict;
            return ModelRoot.ReadGLB(stream, settings);
        }

        private static GameObject CreateGameObject(Node node, Node parent = null)
        {
            if (parent != null)
            {
                var scale = node.LocalTransform.Scale;
                var parentScale = parent.LocalTransform.Scale;
                scale.X *= parentScale.X;
                scale.Y *= parentScale.Y;
                scale.Z *= parentScale.Z;
                var localTransform = node.LocalTransform;
                localTransform.Scale = scale;
                node.LocalTransform = localTransform;
            }

            GameObject gameObject = new GameObject();

            ParseTransform(node, gameObject);
            ParseMesh(node, gameObject);

            foreach(Node child in node.VisualChildren)
            {
                GameObject childObject = CreateGameObject(child,node);
                childObject.transform.parent = gameObject.transform;
            }
            return gameObject;
        }

        private static void ParseTransform(Node node, GameObject gameObject)
        {
            var transform = gameObject.transform;
            var nodeTransform = node.LocalTransform;

            // extension method to Unity transform
            Design.SetTransform(transform, nodeTransform);
        }

        private static void ParseMesh(Node node, GameObject gameObject)
        {
            if (node.Mesh != null)
            {
                //mesh
                var scale = node.LocalTransform.Scale;

                List<Vector3> vertices = new List<Vector3>();
                List<Vector2> uvs = new List<Vector2>();
                List<int> triangles = new List<int>();

                var filter = gameObject.AddComponent<UnityEngine.MeshFilter>();

                foreach (MeshPrimitive primitive in node.Mesh.Primitives)
                {
                    int c = vertices.Count();
                    // checks for POSITION or NORMAL vertex as not all designs have both (TODO: This doesn't trip, if it did would we screw up the triangles?)
                    if (primitive.VertexAccessors.ContainsKey("POSITION"))
                    {
                        var primitiveVertices = primitive.GetVertices("POSITION").AsVector3Array();
                        foreach (var vertex in primitiveVertices)
                            vertices.Add(new Vector3(vertex.X * scale.X, vertex.Y * scale.Y, vertex.Z * scale.Z));
                    }

                    var primitiveTriangles = primitive.GetIndices();
                    for (int i = 0; i < primitiveTriangles.Count; i++)
                        triangles.Add((int)primitiveTriangles[i] + c);
                }

                filter.mesh = new UnityEngine.Mesh();
                filter.mesh.vertices = vertices.ToArray();
                filter.mesh.uv = uvs.ToArray();
                filter.mesh.triangles = triangles.ToArray();
                filter.mesh.RecalculateNormals();

                //material
                var renderer = gameObject.AddComponent<UnityEngine.MeshRenderer>();

            }
        }
    }
}
