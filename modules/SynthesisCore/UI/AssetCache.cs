using SynthesisAPI.AssetManager;

namespace SynthesisCore.UI
{
    public class AssetCache
    {
        public static VisualElementAsset ControlAsset =
            AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Control.uxml");
    }
}