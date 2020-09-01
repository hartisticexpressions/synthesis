using SynthesisAPI.PreferenceManager;

namespace SynthesisCore.UI
{
    public struct ControlInfo
    {
        public string Name { get; }
        public string Key { get; }

        public ControlInfo(string name, string key)
        {
            Name = name;
            Key = key;
        }

        public ControlInfo(string name)
        {
            Name = name;
            Key = GetFormattedPreference(Name);
        }

        private static string GetFormattedPreference(string controlName)
        {
            var controlKey = PreferenceManager.GetPreference("SynthesisCore", controlName);
            if (controlKey is string)
            {
                return Utilities.ReformatCondensedString((string)controlKey);
            }
            return "Unassigned";
        }
    }
}