using UnityEngine;
using System.Collections;

namespace Synthesis.UI.Panels.Variant
{
    public class AddPanel : MonoBehaviour
    {
        public void AddRobot(string filePath)
        {
            ModelManager.ModelManager.Add(filePath);
        }
    }
}
