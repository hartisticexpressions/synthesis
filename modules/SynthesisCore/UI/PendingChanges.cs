using System.Collections.Generic;
using SynthesisAPI.PreferenceManager;

namespace SynthesisCore.UI
{
    public class PendingChanges
    {
        private static Dictionary<string, Dictionary<string, object>> Changes = new Dictionary<string, Dictionary<string, object>>();

        public static void Add(string categoryName, string key, object value)
        {
            if (!Changes.ContainsKey(categoryName))
            {
                Changes[categoryName] = new Dictionary<string, object>();
            }
            Changes[categoryName][key] = value;
        }
        
        public static void ApplyAnyCategoryChanges(string categoryName)
        {
            if (Changes[categoryName].Count > 0)
            {
                foreach (string preferenceName in Changes[categoryName].Keys)
                {
                    PreferenceManager.SetPreference("SynthesisCore", preferenceName, Changes[categoryName][preferenceName]);
                }
                PreferenceManager.Save();
            }
        }
        
        public static Dictionary<string, object> GetCategoryChanges(string categoryName)
        {
            if (Changes.ContainsKey(categoryName))
            {
                return Changes[categoryName];
            }

            return new Dictionary<string, object>();
        }
        
        public static void ClearCategoryChanges(string categoryName)
        {
            if (Changes.ContainsKey(categoryName))
            {
                Changes[categoryName].Clear();
            }
        }
        
    }
}