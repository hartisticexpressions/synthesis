using System;
using SynthesisAPI.Runtime;

namespace SynthesisAPI.Utilities
{
    public static class FileBrowser
    {
        public static string[] GetFiles(string root, string regex)
        {
            return ApiProvider.GetFiles(root, regex);
        }
    }
}
