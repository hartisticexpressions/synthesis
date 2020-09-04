using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;
using SynthesisCore.EntityControl;

namespace SynthesisCore.UI.Windows
{
    public class EntitiesWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;
        private VisualElementAsset EntityAsset;
        private ListView EntityList;

        public EntitiesWindow()
        {
            EntityAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Entity.uxml");

            var entitiesAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Entities.uxml");
            Panel = new Panel("Entities", entitiesAsset, OnWindowOpen);
        }

        private void OnWindowOpen(VisualElement entitiesWindow)
        {
            Window = entitiesWindow;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;
            EntityList = (ListView) Window.Get("entity-list");
            
            LoadWindowContents();
            RegisterButtons();
        }
        
        private void LoadWindowContents()
        {
            EntityImportManager.RefreshModelList();
            // concrete implementation waiting on file browser, currently just using this stub code to show functionality
            foreach (var i in EntityImportManager.GltfAssets)
            {
                EntityList.Add(new EntityItem(EntityAsset, i, new FileInfo(i.Name, "3d")).EntityElement); // TODO date
            }
        }

        private void RegisterButtons()
        {
            // TODO: save / apply changes implementation?

            Button importButton = (Button) Window.Get("import-button");
            importButton?.Subscribe(x => Logger.Log("TODO: Import"));
            
            Button okButton = (Button) Window.Get("ok-button");
            okButton?.Subscribe(x => UIManager.ClosePanel(Panel.Name));

            Button closeButton = (Button) Window.Get("close-button");
            closeButton?.Subscribe(x => UIManager.ClosePanel(Panel.Name));
        }
    }
}