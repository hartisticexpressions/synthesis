using System;
using SharpGLTF.Schema2;
using SharpGLTF.IO;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Synthesis.ModelManager
{
    public static class Parse
    {
        //for testing call -> Synthesis.ModelManager.Parse.AsRobot("Assets/Testing/Test.glb");

        private static bool tryFix = true;

        private const float defaultMass = 1;

        private static Dictionary<string, UnityEngine.Rigidbody> rigidbodyCache = new Dictionary<string, UnityEngine.Rigidbody>();
        private static List<MeshCollider> colliders = new List<MeshCollider>();

        public static GameObject AsRobot(string filePath)
        {
            ModelRoot model = GetModelInfo(filePath);
            GameObject gameObject = CreateGameObject(model.DefaultScene.VisualChildren.First());
            ParseJoints(model);
            /*for(int i = 0; i < colliders.Count; i++)
            {
                for(int j = i + 1; j < colliders.Count; j++)
                {
                    Physics.IgnoreCollision(colliders[i], colliders[j]);
                }
            }*/
            colliders.Clear();
            rigidbodyCache.Clear();
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
            if(node.Mesh != null)
            {
                ParseMesh(node, gameObject);
                ParseMeshCollider(node, gameObject);
                ParseRigidBody(node, gameObject);
            }

            foreach(Node child in node.VisualChildren)
            {
                GameObject childObject = CreateGameObject(child,node);
                childObject.transform.parent = gameObject.transform;
                FixedJoint joint = childObject.AddComponent<FixedJoint>();
                joint.connectedBody = gameObject.GetComponent<Rigidbody>();
            }
            return gameObject;
        }

        private static void ParseTransform(Node node, GameObject gameObject)
        {
            var transform = gameObject.transform;
            var nodeTransform = node.LocalTransform;
            transform.localPosition = new Vector3(nodeTransform.Translation.X * nodeTransform.Scale.X, nodeTransform.Translation.Y * nodeTransform.Scale.Y, nodeTransform.Translation.Z * nodeTransform.Scale.Z);
            transform.localRotation = new Quaternion(nodeTransform.Rotation.X, nodeTransform.Rotation.Y, nodeTransform.Rotation.Z, nodeTransform.Rotation.W);
        }

        private static void ParseMesh(Node node, GameObject gameObject)
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

            //TODO: material
            var renderer = gameObject.AddComponent<UnityEngine.MeshRenderer>();
        }

        private static void ParseMeshCollider(Node node, GameObject gameObject)
        {
            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = true; //outside
            collider.sharedMesh = gameObject.GetComponent<MeshFilter>().mesh; //attach mesh
            colliders.Add(collider);
        }

        private static void ParseRigidBody(Node node, GameObject gameObject)
        {
            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();

            //mass
            if ((node.Mesh.Extras as JsonDictionary).TryGetValue("physicalProperties", out object physicalProperties) && (physicalProperties as JsonDictionary).TryGetValue("mass", out object massStr))
                rigidbody.mass = float.TryParse(massStr.ToString(), out float mass) ? mass : defaultMass;
            
            rigidbodyCache.Add((node.Extras as JsonDictionary)?.Get<string>("uuid"), rigidbody);
        }

        private static void ParseJoints(ModelRoot model)
        {
            var joints = (model.Extras as JsonDictionary)["joints"] as JsonList;

            foreach (var jointObj in joints)
            {
                var joint = jointObj as JsonDictionary;
                string name = joint.Get("header").Get<string>("name"); // Probably not gonna be used
                Vector3 anchor = ParseJointVector3D(joint.Get("origin"));
                Rigidbody parent = rigidbodyCache[joint.Get<string>("occurrenceOneUUID")];
                Rigidbody child = rigidbodyCache[joint.Get<string>("occurrenceTwoUUID")];

                foreach (Joint j in child.gameObject.GetComponents<FixedJoint>())
                {
                    Component.Destroy(j);
                }
                foreach (Joint j in child.gameObject.GetComponents<HingeJoint>())
                {
                    Component.Destroy(j);
                }

                if (joint.ContainsKey("revoluteJointMotion"))
                {
                    var revoluteData = joint.Get("revoluteJointMotion");
                    Vector3 axis = ParseJointVector3D(revoluteData.Get("rotationAxisVector"));
                    // TODO: Support limits
                    HingeJoint result = child.gameObject.AddComponent<HingeJoint>();
                    result.anchor = anchor;
                    result.axis = axis;
                    result.connectedBody = parent;
                }
                else
                { // For yet to be supported and fixed motion joints
                    FixedJoint result = child.gameObject.AddComponent<FixedJoint>();
                    result.anchor = anchor;
                    result.connectedBody = parent;
                }
            }
        }

        private static Vector3 ParseJointVector3D(JsonDictionary dict) =>
           new Vector3((float)dict.TryGet<decimal>("x", 0),
                   (float)dict.TryGet<decimal>("y", 0), (float)dict.TryGet<decimal>("z", 0));
    }
    public static class GltfAssetExtensions
    {
        public static T Get<T>(this JsonDictionary dict, string key) => (T)dict[key];
        public static T TryGet<T>(this JsonDictionary dict, string key, T defaultObj)
        {
            if (dict.ContainsKey(key))
                return (T)dict[key];
            else
                return defaultObj;
        }
        public static JsonDictionary Get(this JsonDictionary dict, string key) => (JsonDictionary)dict[key];

        public static void ForEach<T>(this IEnumerable<T> e, Action<int, T> method)
        {
            for (int i = 0; i < e.Count(); ++i)
                method(i, e.ElementAt(i)); // I really hope this holds reference
        }
    }
}
