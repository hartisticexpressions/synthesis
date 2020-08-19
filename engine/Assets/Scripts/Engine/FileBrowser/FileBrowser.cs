using System;
using System.Runtime.InteropServices;

namespace Engine
{
    public static class FileBrowser
    {
        [DllImport("libfilebrowser", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr SelectFiles(string root, string regex);
        [DllImport("libfilebrowser")]
        private static extern void FreePtr(IntPtr ptr);

        /// <summary>
        /// Opens a file browser window at specified root directory
        /// </summary>
        /// <param name="root">file path of root directory</param>
        /// <param name="regex">accepted file types ex: Images (*.jpg *.png)</param>
        /// <returns>list of chosen files | null if nothing is chosen</returns>
        public static string[] GetFiles(string root, string regex)
        {
            IntPtr ptr = SelectFiles(root, regex); //returns char*
            string s = Marshal.PtrToStringAnsi(ptr); //iterates from pointer until '\0'
            FreePtr(ptr);
            if (s.Length > 0)
                return s.Split(new[] { "<:/:>" }, StringSplitOptions.None); //entries are split by the custom seperator "<:/:>"
            return null;
        }
    }
}