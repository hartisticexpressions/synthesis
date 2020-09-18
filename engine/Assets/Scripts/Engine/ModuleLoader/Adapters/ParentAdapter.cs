using UnityEngine;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.EnvironmentManager;
using static Engine.ModuleLoader.Api;
using System.ComponentModel;

namespace Engine.ModuleLoader.Adapters
{
    public class ParentAdapter : MonoBehaviour, IApiAdapter<Parent>
    {
        private Parent instance;

        public void SetInstance(Parent parent)
        {
            instance = parent;

            instance.PropertyChanged += SetParent;
        }
        private void SetParent(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ParentEntity")
            {
                GameObject _parent = (Entity)instance == 0 ? ApiProviderData.EntityParent : ApiProviderData.GameObjects[instance];
                gameObject.transform.SetParent(_parent.transform);
            }
        }

        public void OnDestroy()
        {
            instance.PropertyChanged -= SetParent;
            instance.Entity.Value.RemoveEntity();
        }

        public static Parent NewInstance()
        {
            return new Parent();
        }
    }
}
