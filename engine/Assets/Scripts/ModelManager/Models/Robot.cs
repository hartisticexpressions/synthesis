using System;
using Synthesis.ModelManager;

namespace Synthesis.ModelManager.Models
{
    public class Robot : Model
    {
        public Robot(string filePath)
        {
            this.gameObject = Parse.AsRobot(filePath);
        }
    }
}