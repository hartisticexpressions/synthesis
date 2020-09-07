using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows
{
    public class ImportWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;
        private VisualElement PageContainer;
        private Label TitleLabel;
        private Label DescriptionLabel;
        
        public ImportWindow()
        {
            var windowAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Import.uxml");
            
            Panel = new Panel("Import", windowAsset, OnWindowCreate);
            
            Button helpButton = (Button)UIManager.RootElement.Get("help-button");
            helpButton.Subscribe(x => UIManager.TogglePanel("Help"));
        }

        private void OnWindowCreate(VisualElement importWindow)
        {
            Window = importWindow;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;

            PageContainer = Window.Get("window-content");
            TitleLabel = (Label) Window.Get("title");
            DescriptionLabel = (Label) Window.Get("description");
            
            RegisterButtons();
        }

        private void RegisterButtons()
        {
            Button cancelButton = (Button) Window.Get("cancel-button");
            cancelButton.Subscribe(x => UIManager.ClosePanel("Import"));

            Button nextPageButton = (Button) Window.Get("next-button");
            // nextPageButton.Subscribe(x => );

        }

    }
}