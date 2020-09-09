using SynthesisAPI.AssetManager;
﻿using System.Collections.Generic;
using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI
{
    public class ControlsPage
    {
        public VisualElement Page { get; }
        private List<ControlItem> ControlsList = new List<ControlItem>();
        private ListView ControlListView;

        public ControlsPage(VisualElementAsset controlsAsset)
        {
            Page = controlsAsset.GetElement("page");
            Page.SetStyleProperty("height", "100%");
            
            ControlListView = (ListView) Page.Get("controls");

            LoadPageContent();
        }

        private void LoadPageContent()
        {
            AddControl("Camera Forward");
            AddControl("Camera Backward");
            AddControl("Camera Left");
            AddControl("Camera Right");
            AddControl("Camera Up");
            AddControl("Camera Down");
            AddControl("Entity Forward");
            AddControl("Entity Backward");
            AddControl("Entity Left");
            AddControl("Entity Right");
            
            ControlsList.ForEach(control => ControlListView.Add(control.Element));
        }

        private void AddControl(string controlName)
        {
            ControlsList.Add(new ControlItem(controlName));
        }

        public void RefreshPreferences()
        {
            ControlsList.ForEach(control => control.UpdateInformation());
        }
    }
}