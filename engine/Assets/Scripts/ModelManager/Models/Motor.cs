using System;
using UnityEngine;

namespace Synthesis.ModelManager.Models
{
    public class Motor : MonoBehaviour
    {
        private HingeJoint _joint;

        public static implicit operator HingeJoint(Motor joint) => joint._joint;

        public Motor(HingeJoint joint)
        {
            _joint = joint;
        }
    }
}