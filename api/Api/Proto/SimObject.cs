using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SynthesisAPI.Proto {
    public partial class SimObject {
        public bool HasParsedMaterials { get; private set; } = false;
        public ReadOnlyDictionary<string, UnityEngine.Material> ParsedMaterials;

        public SimObject ParseMaterials(bool forceParse = false) {
            if (HasParsedMaterials && !forceParse)
                return this;
            var res = new Dictionary<string, UnityEngine.Material>();
            for (int i = 0; i < MaterialRefMap.Count; i++) {
                res.Add(MaterialRefMap.Keys.ElementAt(i), MaterialRefMap.Values.ElementAt(i));
            }
            ParsedMaterials = new ReadOnlyDictionary<string, UnityEngine.Material>(res);
            HasParsedMaterials = true;
            return this;
        }
    }
}
