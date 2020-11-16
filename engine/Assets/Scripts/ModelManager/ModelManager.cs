using System.Collections;
using System.Collections.Generic;
using Synthesis.ModelManager;
using UnityEngine;

namespace Synthesis.ModelManager
{
    public static class ModelManager
    {
        public static void Add(string filePath)
        {
            Parse.AsRobot(filePath);
        }
        public static void Remove()
        {

        }
    }
}

