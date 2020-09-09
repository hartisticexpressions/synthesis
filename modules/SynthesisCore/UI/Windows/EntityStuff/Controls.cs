using SynthesisAPI.UIManager.VisualElements;

namespace SynthesisCore.UI.Windows.EntityStuff
{
    public class Controls
    {
        public VisualElement Content;

        public Controls()
        {
            Content = AssetCache.ContainerAsset.GetElement("container");

            LoadContent();
        }

        private void LoadContent()
        {
            // incorporate entity name into preferenceName, add reference to Entity object
            DropdownItem controlScheme = new DropdownItem("Control Scheme",
                new Dropdown("control-scheme", 0, "Tank", "Arcade"));
            
            ControlItem forwardControl = new ControlItem("Robot Forward");
            ControlItem backwardControl = new ControlItem("Robot Backward");
            ControlItem leftControl = new ControlItem("Robot Left");
            ControlItem rightcontrol = new ControlItem("Robot Right");
            
            Content.Add(controlScheme.Element);
            Content.Add(forwardControl.Element, backwardControl.Element, leftControl.Element, rightcontrol.Element);
        }

    }
}