using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows.EntityStuff
{
    public class ControlsWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;

        public ControlsWindow()
        {
            Panel = new Panel("Controls", AssetCache.WindowAsset, OnWindowCreate);
            
            Button helpButton = (Button) UIManager.RootElement.Get("help-button");
            helpButton.Subscribe(x => UIManager.TogglePanel("Controls"));
        }

        private void OnWindowCreate(VisualElement controlsWindow)
        {
            Window = controlsWindow;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;

            Label titleLabel = (Label) Window.Get("title");
            titleLabel.Text = "Controls";

            VisualElement windowContainer = Window.Get("window");
            windowContainer.SetStyleProperty("width", "330px");
            windowContainer.SetStyleProperty("height", "250px");
            
            VisualElement windowIcon = Window.Get("icon");
            windowIcon.SetStyleProperty("background-image", "/modules/synthesis_core/UI/images/controls-icon-2.png");

            VisualElement windowContent = Window.Get("content");
            windowContent.Add(new Controls().Content);
            
            RegisterButtons();
        }

        private void RegisterButtons()
        {
            Button okButton = (Button) Window.Get("ok-button");
            okButton.Subscribe(x => UIManager.ClosePanel("Controls"));
            
            Button closeButton = (Button) Window.Get("close-button");
            closeButton.Subscribe(x => UIManager.ClosePanel("Controls"));
        }
    }
}