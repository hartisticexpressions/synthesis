using SynthesisAPI.AssetManager;
using SynthesisAPI.EnvironmentManager;
using SynthesisAPI.EnvironmentManager.Components;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;
using SynthesisCore.Components;
using SynthesisCore.EntityControl;
using SynthesisCore.EntityMovement;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SynthesisCoreData;

namespace SynthesisCore.UI.Windows
{
    public class EntityItem
    {
        public VisualElement EntityElement { get; }
        private GltfAsset ModelAsset { get; }

        private static bool firstSpawn = true;

        public EntityItem(VisualElementAsset entityAsset, GltfAsset modelAsset, FileInfo fileInfo)
        {
            EntityElement = entityAsset.GetElement("entity");

            ModelAsset = modelAsset;

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
            
            deleteButton.Subscribe(x =>
            {
                EntityElement.RemoveFromHierarchy();
                // TODO delete from registered entities (to come)
            });
            
            spawnButton.Subscribe(x =>
            {
                var e = SimulationManager.SpawnEntity(ModelAsset, Path.GetFileNameWithoutExtension(ModelAsset.Name));
                var eName = e.GetComponent<Name>();

                MoveArrows.MoveEntity(e);
                if (firstSpawn)
                {
                    Logger.Log("Press Return or Escape to stop moving entity");
                    firstSpawn = false; // TODO put this in preferences so it's tracked through application launches
                    
                    if (eName.IsUnique)
                    {
                        var t = ModelsDict[eName.Value];
                        foreach (var name in ModelsDict)
                        {
                            Logger.Log("Entity " + name.Entity);
                            Logger.Log("Value " + name.Value);
                        }
                    }
                }
            });
        }

    }
}