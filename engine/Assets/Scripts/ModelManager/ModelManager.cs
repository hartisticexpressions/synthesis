using System.Collections;
using System.Collections.Generic;
using System.IO;
using Synthesis.ModelManager;
using Synthesis.ModelManager.Models;
using UnityEngine;

namespace Synthesis.ModelManager
{
    public static class ModelManager
    {
        private static Dictionary<string, int> ModelNames = new Dictionary<string, int>();
        public static Dictionary<string, Dictionary<string,Model>>  Models = new Dictionary<string, Dictionary<string, Model>>();

        public static Field Field { get; set; }

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            string filePath = FileSystem.FileSystem.Robots;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            else foreach (string model in Directory.GetFiles(filePath, "*.g*"))
                Models.Add(Path.GetFileNameWithoutExtension(model),new Dictionary<string, Model>());
        }

        public static void AddModel(string filePath)
        {
            Model m = Parse.AsModel(filePath);

            if (!Models.ContainsKey(m.ObjectName))
            {
                Models.Add(m.ObjectName, new Dictionary<string, Model>());
                ModelNames.Add(m.ObjectName, 0);
            }

            m.InstanceName = m.ObjectName + " " + ++ModelNames[m.ObjectName];
            
            Models[m.ObjectName].Add(m.InstanceName,m);
        }

        public static void SetField(string filePath)
        {
            Field = Parse.AsField(filePath);
        }

        public static void RemoveAll(string modelName)
        {
            if (!Models.ContainsKey(modelName)) return;
            Models.Remove(modelName);
        }

        public static void Remove(string instanceName)
        {
            foreach (Dictionary<string, Model> dict in Models.Values)
                if (dict.ContainsKey(instanceName)){
                    dict.Remove(instanceName);
                }
        }

        public static void Remove(this Model model)
        {
            if (Models.ContainsKey(model.ObjectName) && Models[model.ObjectName].ContainsKey(model.InstanceName))
            {
                Models[model.ObjectName].Remove(model.InstanceName);
            }
        }
    }
}

