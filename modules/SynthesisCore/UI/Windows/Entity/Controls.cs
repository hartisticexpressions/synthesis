using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows.Entity
{
    public class Controls
    {
        public VisualElement Content { get; }
        
        public Controls(VisualElement container)
        {


            VisualElement test = AssetCache.ControlAsset.GetElement("control");

            LoadContent();
        }

        private void LoadContent()
        {
            
        }
    }
}