using UnityEngine;
using System.Collections;

namespace Synthesis.UI.NavBar
{
    public class NavBar : MonoBehaviour
    {
        public void OpenRobotPanel()
        {
            Synthesis.ModelManager.Parse.AsRobot("Assets/Testing/Test.glb");
        }
        public void OpenFieldPanel()
        {
            Debug.Log("Open Field Panel");
        }
    }
}
