using UnityEngine;

namespace Synthesis.Physics
{
    /// <summary>
    /// Controls physics within unity
    /// </summary>
    public class PhysicsManager : MonoBehaviour
    {
        public static bool isPaused { get; set; } = false;

        // Use this for initialization
        void Awake()
        {
            UnityEngine.Physics.autoSimulation = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(!isPaused) StepForward();
        }

        public static void Pause()
        {
            isPaused = true;
        }

        public static void Play()
        {
            isPaused = false;
        }

        public static void TogglePlay()
        {
            isPaused = !isPaused;
        }
        public static void StepForward()
        {
            UnityEngine.Physics.Simulate(Time.fixedDeltaTime);
        }
        public static void StepBackward()
        {
            //TODO: save position and rotation of objects each frame?
        }
    }
}