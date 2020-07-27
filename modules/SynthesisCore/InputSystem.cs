﻿using System;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.InputManager;
using SynthesisAPI.Modules.Attributes;

namespace SynthesisCore
{
    [ModuleExport]
    public class InputSystem : SystemBase
    {
        public InputSystem() { }

        public override void Setup() { }

        public override void OnPhysicsUpdate() { }

        public override void OnUpdate()
        {
            InputManager.UpdateInputs();
        }
    }
}
