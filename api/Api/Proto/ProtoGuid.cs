using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SynthesisAPI.Proto {
    /// <summary>
    /// Partial class to add utility functions and properties to Protobuf types
    /// </summary>
    public partial class ProtoGuid {
        // public static implicit operator Guid(ProtoGuid g) => new Guid(g.Guid);
        // public static implicit operator ProtoGuid(Guid g) => new ProtoGuid() { Guid = g.ToString() };
    }
}
