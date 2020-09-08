using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager;
using SynthesisAPI.UIManager.UIComponents;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisAPI.Utilities;

namespace SynthesisCore.UI.Windows
{
    public class ImportWindow
    {
        public Panel Panel { get; }
        private VisualElement Window;
        private Label TitleLabel;
        private Label DescriptionLabel;
        private VisualElement PageContainer;
        private Button PreviousPageButton;
        private Button NextPageButton;
        private Button CancelButton;
        
        private int CurrentPageIndex;
        
        public ImportWindow()
        {
            var windowAsset = AssetManager.GetAsset<VisualElementAsset>("/modules/synthesis_core/UI/uxml/Import.uxml");
            
            Panel = new Panel("Import", windowAsset, OnWindowCreate);

            // TEMPORARY
            Button helpButton = (Button)UIManager.RootElement.Get("help-button");
            helpButton.Subscribe(x => UIManager.TogglePanel("Import"));
            //
        }

        private void OnWindowCreate(VisualElement importWindow)
        {
            Window = importWindow;
            Window.SetStyleProperty("position", "absolute");
            Window.IsDraggable = true;
            
            TitleLabel = (Label) Window.Get("title");
            DescriptionLabel = (Label) Window.Get("description");
            PageContainer = Window.Get("window-content");
            
            CancelButton = (Button) Window.Get("cancel-button");
            PreviousPageButton = (Button) Window.Get("previous-button");
            NextPageButton = (Button) Window.Get("next-button");

            RegisterButtons();
            LoadWindowContent();
        }

        private void LoadWindowContent()
        {
            SetPage(1);
        }

        private void SetPage(int desiredIndex)
        {
            // 1. drivetrain type
            // changes what displays in controls
            // 2. joint editor
            // 3. set wheels
            
            if (desiredIndex <= 1)
            {
                DescriptionLabel.SetStyleProperty("font-size", "15px");
                DescriptionLabel.Text = "A new unconfigured asset with Joints has been detected. It is recommended " +
                                        "to use this Wizard if this asset is a robot or vehicle. These options " +
                                        "can be modified again later in its individual object tab.";
                
                PreviousPageButton.Enabled = false;
                CancelButton.SetStyleProperty("visibility", "visible");
                TitleLabel.Text = "Import Asset";
                CurrentPageIndex = 1;

            } else if (desiredIndex == 2)
            {
                DescriptionLabel.SetStyleProperty("font-size", "13px");
                DescriptionLabel.Text = "Step 1";

                CancelButton.SetStyleProperty("visibility", "visible");
                TitleLabel.Text = "Import Asset: Step 1 of 3";
                PreviousPageButton.Enabled = true;

            } else if (desiredIndex == 3)
            {
                DescriptionLabel.SetStyleProperty("font-size", "13px");
                DescriptionLabel.Text = "Step 2";
                
                CancelButton.SetStyleProperty("visibility", "visible");
                TitleLabel.Text = "Import Asset: Step 2 of 3";
                PreviousPageButton.Enabled = true;

            } else if (desiredIndex == 4) {
                
                DescriptionLabel.SetStyleProperty("font-size", "13px");
                DescriptionLabel.Text = "Step 3";

                DescriptionLabel.Text = "Control Scheme and Controls | In order for Synthesis to know how to control " +
                                        "your robot or vehicle you must specify a control scheme. Configuring your " +
                                        "Controls are optional and the default values are sufficient. These controls " +
                                        "are specific to this robot or vehicle.";
                
                CancelButton.SetStyleProperty("visibility", "visible");
                TitleLabel.Text = "Import Asset: Step 3 of 3";
                PreviousPageButton.Enabled = true;
                
            } else
            {
                DescriptionLabel.SetStyleProperty("font-size", "15px");
                DescriptionLabel.Text = "The import process has been completed. These settings have been saved for this " +
                                        "object and do not have to be set again if the design is re-exported. These " +
                                        "settings can be changed again by clicking the object in the simulator and " +
                                        "switching to its tab in the toolbar.";
                
                PreviousPageButton.SetStyleProperty("visibility", "hidden");
                NextPageButton.Text = "Finish";

                CancelButton.SetStyleProperty("visibility", "hidden");
                TitleLabel.Text = "Import Asset: Complete";
                PreviousPageButton.Enabled = true;
                CurrentPageIndex = 5;
            }
        }

        private void RegisterButtons()
        {
            CancelButton.Subscribe(x => UIManager.ClosePanel("Import"));
            PreviousPageButton.Subscribe(x => SetPage(--CurrentPageIndex));
            
            NextPageButton.Subscribe(x =>
            {
                if (NextPageButton.Text.Equals("Next"))
                {
                    SetPage(++CurrentPageIndex);
                } else if (NextPageButton.Text.Equals("Finish"))
                    UIManager.ClosePanel("Import");
            });
        }

    }
}