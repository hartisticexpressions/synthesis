using SynthesisAPI.EnvironmentManager;
using System.Linq;

namespace SynthesisCore.Components
{
    public class Name : Component
    {
        public string Value { get; set; }

        public bool IsUnique => EnvironmentManager.GetComponentsWhere<Name>(c => c.Value == Value).Count() == 1;
    }
}
