using System;
using UnityEngine;

namespace Synthesis.ModelManager.Models
{
    public class Motor : MonoBehaviour
    {
        public float MaxSpeed;
        public float Torque;

        private HingeJoint _joint;
        public HingeJoint Joint { get => _joint; set => _joint = value; }

        public static implicit operator HingeJoint(Motor joint) => joint._joint;
    }
}