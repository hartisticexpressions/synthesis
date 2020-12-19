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
        public HashSet<Motor> Motors { get => motors; } // TODO: This bad, go back and fix
        public List<GearboxData> GearboxMeta = new List<GearboxData>();

        public DrivetrainType DrivetrainType;

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
            m.Joint = joint;
            return motors.Add(m);
        }
    }

    public struct DrivetrainMeta
    {
        public DrivetrainType Type;
        public List<GearboxData> SelectedGearboxes;
    }

    public enum DrivetrainType
    {
        Arcade, Tank
    }
}