using System;
using UnityEngine;

namespace Synthesis.ModelManager.Models
{
    public class Motor : MonoBehaviour
    {
        private HingeJoint _joint;
        public HingeJoint Joint { get => _joint; set => _joint = value; }

        public (string Name, string Uuid) Meta => _joint.GetComponent<JointMeta>().Data;

        public static implicit operator HingeJoint(Motor joint) => joint._joint;

        public Motor(HingeJoint joint)
        {
            _joint = joint;
        }
    }
}