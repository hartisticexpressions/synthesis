using System;

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