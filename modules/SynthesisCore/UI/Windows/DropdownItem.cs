using SynthesisAPI.PreferenceManager;
using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows
{
    public class DropdownItem
    {
        public VisualElement Element { get; }
        private string PreferenceName;
        private Dropdown Dropdown;
        private Label NameLabel;
        private VisualElement ModifierContainer;

        public DropdownItem(string preferenceName, Dropdown dropdown)
        {
            PreferenceName = preferenceName;
            Dropdown = dropdown;
          
            Element = AssetCache.OptionAsset.GetElement("option");
            NameLabel = (Label) Element.Get("option-name");
            ModifierContainer = Element.Get("modifier-container");

            UpdateInformation();
            RegisterButtons();
        }

        public void UpdateInformation()
        {
            NameLabel.Text = PreferenceName;
            Dropdown.Selected = GetPreference();

            foreach (VisualElement child in ModifierContainer.GetChildren())
            {
                child.RemoveFromHierarchy();
            }
            ModifierContainer.Add(Dropdown);
        }

        private void RegisterButtons()
        {
            Dropdown.Subscribe(e =>
            {
                if (e is Dropdown.SelectionEvent selectionEvent)
                {
                    PendingChanges.Add("Settings", PreferenceName, selectionEvent.SelectionName);
                }
            });
        }

        private string GetPreference()
        {
            return PreferenceManager.GetPreference<string>("SynthesisCore", PreferenceName);
        }
    }
}