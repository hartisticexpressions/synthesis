using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows
{
    public class RobotControlsPage
    {
        public VisualElement Page { get; }
        private ListView controlList;
        private Dropdown entityDropdown;

        public RobotControlsPage(VisualElementAsset controlsAsset)
        {
            Page = controlsAsset.GetElement("page");
            Page.SetStyleProperty("height", "100%");

            controlList = (ListView)Page.Get("robot-controls");

            LoadPageContent();
        }

        private void LoadPageContent()
        {
            var entitySelector = Page.Get("entity-selector-dropdown-container");
            entityDropdown = new Dropdown("entity-dropdown", 0, SynthesisCoreData.ModelsDict.Keys);
            entitySelector.Add(entityDropdown);

            // TODO
        }
    }
}