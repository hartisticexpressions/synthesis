using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows.EntityStuff
{
    public class Controls
    {
        public VisualElement Content { get; }
        
        public Controls(VisualElement destinationContainer)
        {
            Content = AssetCache.ControlAsset.GetElement("control");
            destinationContainer.Add(Content);
            
            LoadContent();
        }

        private void LoadContent()
        {
            
        }
    }
}