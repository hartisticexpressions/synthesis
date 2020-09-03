using SynthesisAPI.Modules.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SynthesisAPI.EnvironmentManager.Components
{
    public class Parent : Component
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal Entity parentEntity = 0;
        public static implicit operator Entity(Parent p) => p.parentEntity;
        //TODO wont set if entity does not exist

        public Entity ParentEntity {
            get => parentEntity;
            set {
                parentEntity = value;
                OnPropertyChanged();
            }
        }

        public bool IsDescendant(Entity ancestor)
        {
            return IsDescendant(ancestor, Entity.Value);
        }

        public static bool IsDescendant(Entity ancestor, Entity test)
        {
            if (test == ancestor)
                return true;
            var parent = test.GetComponent<Parent>()?.ParentEntity;
            return parent != null && IsDescendant(ancestor, parent.Value);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
