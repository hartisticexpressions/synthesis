using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;
using SynthesisCore.EntityMovement;

namespace SynthesisCore.UI.Windows
{
    public class EntityItem
    {
        public VisualElement EntityElement { get; }
        private bool isStaticEntity = true;
        private bool isStaticEnvironment = false;
        EntityStaticity entityStaticity = new EntityStaticity();

        public EntityItem(VisualElementAsset entityAsset, FileInfo fileInfo)
        {
            EntityElement = entityAsset.GetElement("entity");
            
            SetInformation(fileInfo);
            RegisterButtons();
        }

        private void SetInformation(FileInfo fileInfo)
        {
            var nameLabel = (Label) EntityElement.Get("name");
            var lastModifiedLabel = (Label) EntityElement.Get("last-modified-date");
            
            nameLabel.Text = nameLabel.Text.Replace("%name%", fileInfo.Name);
            lastModifiedLabel.Text = lastModifiedLabel.Text.Replace("%time%", fileInfo.LastModified);
        }
        
        private void RegisterButtons()
        {
            Button deleteButton = (Button) EntityElement.Get("delete-button");
            Button spawnButton = (Button) EntityElement.Get("spawn-button");
            
            // trash can button to delete Environment from imported list
            deleteButton.Subscribe(x =>
            {
                EntityElement.RemoveFromHierarchy();
                // delete from registered entities (to come)
            });
            
            // check mark button to set Environment
            spawnButton.Subscribe(x =>
            {
                Logger.Log("Spawn entity");
                // implementation to add entity from file reference (to come)

                DialogInfo dialogInfo = new DialogInfo();
                dialogInfo.Title = "Entity or Environment Settings";
                dialogInfo.Prompt = "Please set object type.";
                dialogInfo.Description =
                    "Is this object an entity or environment?";
                dialogInfo.SubmitButtonText = "Entity";
                dialogInfo.CloseButtonText = "Environment";
                dialogInfo.SubmitButtonAction = ev =>
                {
                    entityStaticity.SetEntityStaticity(true);
                    isStaticEntity = true;
                    isStaticEnvironment = false;
                    Logger.Log("Entity button clicked");
                };
                dialogInfo.CloseButtonAction = ev =>
                {
                    entityStaticity.SetEntityStaticity(false);
                    isStaticEntity = false;
                    isStaticEnvironment = true;
                    Logger.Log("Close button clicked");
                };
                Dialog.SendDialog(dialogInfo);
            });
        }

    }
}