using System.Collections.Generic;
using System.Linq;
using SynthesisAPI.AssetManager;
using SynthesisAPI.EventBus;
using SynthesisAPI.PreferenceManager;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisCore.UI.Windows;

namespace SynthesisCore.UI
{
    public class SettingsWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;
        private VisualElement pageContainer;
        private GeneralPage GeneralPage;
        private ControlsPage ControlsPage;

        private static Dictionary<string, object> PendingChanges = new Dictionary<string, object>();
        
        public SettingsWindow()
        {
            var generalAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/General.uxml");
            var controlsAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Controls.uxml");
            var robotControlsAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/RobotControls.uxml");

            GeneralPage = new GeneralPage(generalAsset);
            ControlsPage = new ControlsPage(controlsAsset);
            RobotControlsPage.Create(robotControlsAsset);

            var settingsAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Settings.uxml");
            Panel = new Panel("Settings", settingsAsset, OnWindowCreate);

            Button settingsButton = (Button) UIManager.RootElement.Get("settings-button");
            settingsButton.Subscribe(x => UIManager.TogglePanel("Settings"));
            
            SetupWindowToggleCallbacks();
        }

        private void OnWindowCreate(VisualElement settingsWindow)
        {
            Window = settingsWindow;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;
            pageContainer = Window.Get("page-container");

            RegisterButtons();
            LoadWindowContent();
        }

        private void SetupWindowToggleCallbacks()
        {
            EventBus.NewTypeListener<ShowPanelEvent>(info =>
            {
                if (info != null && info.Panel.Name.Equals("Settings"))
                {
                    if(pageContainer.GetChildren().Any(e => e.Name == "robot-controls-page"))
                        RobotControlsPage.LookAtEntity();
                }
            });

            EventBus.NewTypeListener<ClosePanelEvent>(info =>
            {
                if (info != null && info.Panel.Name.Equals("Settings"))
                {
                    GeneralPage.RefreshPreferences();
                    ControlsPage.RefreshPreferences();
                    RobotControlsPage.RefreshPreferences();
                    RobotControlsPage.StopLookAtEntity();
                }
            });
        }
        
        private void LoadWindowContent()
        {
            SetPageContent(GeneralPage.Page);
        }

        private void RegisterButtons()
        {
            Button generalSettingsButton = (Button) Window.Get("general-settings-button");
            generalSettingsButton?.Subscribe(x =>
            {
                SetPageContent(GeneralPage.Page);
            });
            
            Button controlsSettingsButton = (Button) Window.Get("controls-settings-button");
            controlsSettingsButton?.Subscribe(x =>
            {
                SetPageContent(ControlsPage.Page);
            });

            Button robotControlsSettingsButton = (Button)Window.Get("robot-controls-settings-button");
            robotControlsSettingsButton?.Subscribe(x =>
            {
                SetPageContent(RobotControlsPage.Page);
                RobotControlsPage.LookAtEntity();
            });

            Button okButton = (Button) Window.Get("ok-button");
            okButton?.Subscribe(x =>
            {
                if (PendingChanges.Count > 0)
                {
                    foreach (string key in PendingChanges.Keys)
                    {
                        PreferenceManager.SetPreference("SynthesisCore", key, PendingChanges[key]);
                    }
                }
                PreferenceManager.Save();

                OnPageChange();
                UIManager.ClosePanel(Panel.Name);
            });

            Button closeButton = (Button) Window.Get("close-button");
            closeButton?.Subscribe(x =>
            {
                PendingChanges.Clear();

                OnPageChange();
                UIManager.ClosePanel(Panel.Name);
            });
        }

        private void SetPageContent(VisualElement newContent)
        {
            foreach (VisualElement child in pageContainer.GetChildren())
            {
                child.RemoveFromHierarchy();
            }
            pageContainer.Add(newContent);
            OnPageChange();
        }

        public static void AddPendingChange(string key, object value)
        {
            PendingChanges[key] = value;
        }

        private void OnPageChange()
        {
            RobotControlsPage.StopLookAtEntity();
        }
    }
}