using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ModelManager
{
    public static class Design
    {
        public static void SetTransform(Transform transform, SharpGLTF.Transforms.AffineTransform nodeTransform)
        {
            Vector3 position = transform.localPosition;
            Quaternion quat = transform.localRotation;

            transform.localPosition = new Vector3(nodeTransform.Translation.X * nodeTransform.Scale.X, nodeTransform.Translation.Y * nodeTransform.Scale.Y, nodeTransform.Translation.Z * nodeTransform.Scale.Z);
            transform.localRotation = new Quaternion(nodeTransform.Rotation.X, nodeTransform.Rotation.Y, nodeTransform.Rotation.Z, nodeTransform.Rotation.W);

            transform.localPosition = position;
            transform.localRotation = quat;
        }

        public struct Vector3
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

            public Vector3(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public static implicit operator UnityEngine.Vector3(Vector3 vec) =>
                new UnityEngine.Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
            public static implicit operator Vector3(UnityEngine.Vector3 vec) =>
                new Vector3(vec.x, vec.y, vec.z);
        }
        public struct Vector2
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Vector2(double x, double y)
            {
                X = x;
                Y = y;
            }

            public static implicit operator UnityEngine.Vector2(Vector2 vec) =>
                new UnityEngine.Vector2((float)vec.X, (float)vec.Y);
            public static implicit operator Vector2(UnityEngine.Vector2 vec) =>
                new Vector2(vec.x, vec.y);
        }
    }
}
