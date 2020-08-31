using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisCore.UI.Windows;

namespace SynthesisCore.UI
{
    public class GeneralPage
    {
        public VisualElement Page { get; }
        private VisualElementAsset OptionAsset;

        public GeneralPage(VisualElementAsset generalAsset)
        {
            Page = generalAsset.GetElement("page");
            OptionAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Option.uxml");
            
            LoadPageContent();
        }

        private void LoadPageContent()
        {
            Page.Add(new ToggleItem(OptionAsset, new Toggle("analytics"), "Analytics").Element);
            
            Page.Add(new DropdownItem(OptionAsset,
                new Dropdown("measurement-units", 0, "Metric", "Imperial"), "Measurement Units").Element);
            
            Dropdown qualityLevel = new Dropdown("quality", 0,
                "Low", "Medium", "High", "Ultra");
            Page.Add(new DropdownItem(OptionAsset, qualityLevel, "Quality").Element);

            Dropdown resolution = new Dropdown("resolution", 0,
                "1280x720", "1280x768", "1280x800", "1280x1024", "1360x768", "1366x768",
                "1400x1050", "1440x900", "1600x900", "1680x1050", "1920x1080");
            Page.Add(new DropdownItem(OptionAsset, resolution, "Screen Resolution").Element);
            
        }
    }
}