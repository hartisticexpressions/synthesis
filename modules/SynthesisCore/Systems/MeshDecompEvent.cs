using System;
using System.Collections.Generic;
using System.Text;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.EventBus;
namespace SynthesisCore.Systems
{
    class MeshDecompEvent : IEvent
    {
        public Mesh inputMesh = null;
    }

    class MeshDecompFinishedEvent : IEvent
    {
        public Mesh resultMesh;
        public double time;
    }
}
