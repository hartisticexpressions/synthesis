using System;
using System.Collections.Generic;
using UnityEngine;

namespace Synthesis.ModelManager.Models
{
    public class Model
    {
        public string Name { get; set; }

        public GameObject GameObject { get; set; }

        protected HashSet<Motor> motors = new HashSet<Motor>();

        public static implicit operator GameObject(Model model) => model.GameObject;

        public Model()
        {

        }

        public Model(string filePath)
        {
            Parse.AsModel(filePath, this);
        }

        public bool AddMotor(HingeJoint joint)
        {
            Motor m = GameObject.AddComponent<Motor>();
            return motors.Add(new Motor(joint));
        }
    }
}