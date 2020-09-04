using SynthesisAPI.AssetManager;
using SynthesisAPI.VirtualFileSystem;
using System.IO;

namespace SynthesisCore.EntityControl
{
    public static class ConfigManager
    {
        public readonly static string ConfigPath = SynthesisAPI.VirtualFileSystem.Directory.DirectorySeparatorChar + "config";
        public readonly static string SourceConfigPath = "config" + Path.DirectorySeparatorChar;

        public static void Test(string name)
        {
            var test = AssetManager.Import<JsonAsset>("text/json", true, ConfigPath, name, Permissions.PublicReadWrite, SourceConfigPath + name);
        }
    }
}
