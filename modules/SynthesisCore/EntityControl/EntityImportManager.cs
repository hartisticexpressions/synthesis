using SynthesisAPI.AssetManager;
using SynthesisAPI.Utilities;
using SynthesisAPI.VirtualFileSystem;
using System;
using System.Collections.Generic;

namespace SynthesisCore.EntityControl
{
    public static class EntityImportManager
    {
        public static List<GltfAsset> GltfAssets { get; private set; } = new List<GltfAsset>();

        public static void RefreshModelList()
        {
            GltfAssets.Clear();
            TraverseAll(FileSystem.Traverse<Directory>("/"), e => {
                if(e is GltfAsset gltfAsset)
                {
                    GltfAssets.Add(gltfAsset);
                }
            });
        }

        private static void TraverseAll(Directory dir, Action<IEntry> action)
        {
            action(dir);
            foreach (var i in dir)
            {
                if (i.Key == "." || i.Key == "..")
                {
                    continue;
                }

                if (i.Value is Directory nextDir)
                {
                    TraverseAll(nextDir, action);
                }
                else
                {
                    action(i.Value);
                }
            }
        }
    }
}
