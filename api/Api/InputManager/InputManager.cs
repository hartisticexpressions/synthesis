using System;
using System.Collections.Generic;
using SynthesisAPI.InputManager.InputEvents;
using SynthesisAPI.InputManager.Inputs;

namespace SynthesisAPI.InputManager
{
    public static class InputManager
    {
        internal static Dictionary<string, List<Digital>> _mappedDigitalInputs = new Dictionary<string, List<Digital>>();
        private static Dictionary<string, Analog> _mappedAxisInputs = new Dictionary<string, Analog>();

        public static void AssignDigitalInput(string controlName, Digital input, EventBus.EventBus.EventCallback callback = null) // TODO remove callback argument?
        {
            _mappedDigitalInputs[controlName] = new List<Digital> { input };
            if (callback != null)
                EventBus.EventBus.NewTagListener($"input/{controlName}", callback);
        }

        public static void UnassignDigitalInput(string controlName, Digital input)
        {
            if (_mappedDigitalInputs.ContainsKey(controlName))
            {
                _mappedDigitalInputs[controlName].Remove(input);
                if (_mappedDigitalInputs[controlName].Count == 0)
                {
                    UnassignDigitalInput(controlName);
                }
            }
        }

        public static void UnassignDigitalInput(string controlName)
        {
            _mappedDigitalInputs.Remove(controlName);
            EventBus.EventBus.RemoveAllTagListeners($"input/{controlName}");
        }

        public static void AssignDigitalInputs(string controlName, List<Digital> input, EventBus.EventBus.EventCallback callback = null)
        {
            _mappedDigitalInputs[controlName] = input;
            if (callback != null)
                EventBus.EventBus.NewTagListener($"input/{controlName}", callback);
        }

        public static void AssignAxis(string controlName, Analog axis)
        {
            _mappedAxisInputs[controlName] = axis;
        }

        public static void UnassignAxis(string controlName)
        {
            _mappedAxisInputs.Remove(controlName);
        }

        public static void UpdateInputs()
        {
            foreach (string name in _mappedDigitalInputs.Keys)
            {
                foreach (Input input in _mappedDigitalInputs[name])
                {
                    if (!input.Name.EndsWith("non-ui") && input.Update())
                    {
                        if (input is MouseDown mouseDown)
                        {
                            EventBus.EventBus.Push($"input/{name}", new MouseDownEvent(name, mouseDown.State, mouseDown.MousePosition));
                        }
                        else if (input is Digital digitalInput)
                        {
                            EventBus.EventBus.Push($"input/{name}", new DigitalEvent(name, digitalInput.State));
                        }
                    }
                }
            }
        }

        public static float GetAxisValue(string name)
        {
            if (_mappedAxisInputs.ContainsKey(name))
            {
                _mappedAxisInputs[name].Update();
                return _mappedAxisInputs[name].Value;
            }
            throw new Exception($"Axis value is not mapped with name \"{name}\"");
        }

        public static void SetAllInputs(Dictionary<string, List<Digital>> input)
        {
            _mappedDigitalInputs = input;
        }

        public static Dictionary<string, List<Digital>> GetAllInputs()
        {
            return _mappedDigitalInputs;
        }
    }
}