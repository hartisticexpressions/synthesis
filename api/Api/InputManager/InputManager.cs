using System;
using System.Collections.Generic;
using SynthesisAPI.InputManager.InputEvents;
using SynthesisAPI.InputManager.Inputs;
using Input = SynthesisAPI.InputManager.Inputs.Input;

namespace SynthesisAPI.InputManager
{
    public static class InputManager
    {
        internal static Dictionary<string, Input[]> _mappedInputs = new Dictionary<string, Input[]>();

        public static void AssignInput(string name, Input input, EventBus.EventBus.EventCallback callback = null) // TODO remove callback argument?
        {
            _mappedInputs[name] = new Input[] { input };
            if (callback != null)
                EventBus.EventBus.NewTagListener($"input/{name}", callback);
        }

        public static void AssignInputs(string name, Input[] input, EventBus.EventBus.EventCallback callback = null)
        {
            _mappedInputs[name] = input;
            if (callback != null)
                EventBus.EventBus.NewTagListener($"input/{name}", callback);
        }

        public static void UnassignInput(string name)
        {
            _mappedInputs.Remove(name);
            EventBus.EventBus.RemoveAllTagListeners($"input/{name}");
        }

        public static void UpdateInputs()
        {
            foreach(string name in _mappedInputs.Keys)
            {
                foreach(Input input in _mappedInputs[name])
                {
                    if (!input.Name.EndsWith("non-ui") && input.Update())
                    {
                        if (input is MouseScroll onMouseScroll)
                            EventBus.EventBus.Push($"input/{name}", new MouseScrollEvent(input.Name, onMouseScroll.Value, onMouseScroll.MousePosition));
                        if (input is MouseDown mouseDown)
                        {
                            EventBus.EventBus.Push($"input/{name}", new MouseDownEvent(input.Name, mouseDown.State, mouseDown.MousePosition));
                        }
                        else if (input is Digital digitalInput)
                        {
                            EventBus.EventBus.Push($"input/{name}", new DigitalEvent(input.Name, digitalInput.State));
                        }
                    }
                }
            }
        }

        public static DigitalState GetDigitalState(string name)
        {
            Digital d = new Digital(name);
            d.Update();
            return d.State;
        }

        public static float GetAnalogValue(string name)
        {
            Analog a = new Analog(name);
            a.Update();
            return a.Value;
        }

        public static void SetAllInputs(Dictionary<string, Input[]> input)
        {
            _mappedInputs = input;
        }

        public static Dictionary<string, Input[]> GetAllInputs()
        {
            return _mappedInputs;
        }
    }
}
