using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.Utilities;
using SynthesisCore.Components;

public static class SynthesisCoreData
{
    public class ModelsDictionary : IEnumerable<Name>
    {
        public Entity? this[string name]
        {
            get
            {
                var list = EnvironmentManager.GetComponentsWhere<Name>(c => c.Value == name);
                if (list.Count() > 0)
                {
                    if(list.Count() > 1)
                    {
                        Logger.Log($"Retrieved first entity with name {name} of {list.Count()} named entities", LogLevel.Debug);
                    }
                    return list.First().Entity;
                }
                return null;
            }
        }

        public IEnumerator<Name> GetEnumerator()
        {
            return EnvironmentManager.GetComponentsWhere<Name>(_ => true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static ModelsDictionary ModelsDict = new ModelsDictionary(); // TODO rework this
}
