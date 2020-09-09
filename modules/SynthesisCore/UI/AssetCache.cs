using SynthesisAPI.AssetManager;

namespace SynthesisCore.UI
{
    public class AssetCache
    {
        public static VisualElementAsset ContainerAsset =
            AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Container.uxml");

        public static VisualElementAsset OptionAsset =
            AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Option.uxml");
        
        public static VisualElementAsset ControlAsset =
            AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Control.uxml");

        public static VisualElementAsset WindowAsset =
            AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Window.uxml");


    }
}