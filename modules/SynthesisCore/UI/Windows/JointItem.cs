using SynthesisAPI.AssetManager;
using SynthesisAPI.UIManager.VisualElements;
using SynthesisCore.Simulation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SynthesisCore.UI
{
    public class JointItem
    {
        private static List<JointItem> jointItems = new List<JointItem>();
        private bool IsHighlighted = false;
        private Button highlightButton;

        public VisualElement JointElement { get; }
        
        public JointItem(VisualElementAsset jointAsset, MotorAssembly assembly) : this(jointAsset)
        {
            RegisterJointButtons(assembly);
        }

        public JointItem(VisualElementAsset jointAsset)
        {
            jointItems.Add(this);
            JointElement = jointAsset.GetElement("joint");
        }

        private void RegisterJointButtons(MotorAssembly assembly)
        {
            var j = assembly.Joint;
            var jointEntity = assembly.Entity;

            highlightButton = (Button)JointElement.Get("highlight-button");
            Dropdown motorTypeDropdown = new Dropdown("motor-type-dropdown-container", MotorTypes.AllTypeNames.IndexOf(assembly.Motor.Name), MotorTypes.AllTypeNames)
            {
                ItemHeight = 15
            };
            JointElement.Get("motor-type-dropdown-container").Add(motorTypeDropdown);
            TextField gearField = (TextField)JointElement.Get("motor-gear-field");
            TextField countField = (TextField)JointElement.Get("motor-count-field");
            
            gearField.SetValueWithoutNotify(assembly.GearReduction.ToString());
            countField.SetValueWithoutNotify(assembly.MotorCount.ToString());

            motorTypeDropdown.Subscribe(e =>
            {
                if (e is Dropdown.SelectionEvent selectionEvent)
                {
                    if (MotorTypes.Contains(selectionEvent.SelectionName))
                    {
                        assembly.Motor = MotorTypes.Get(selectionEvent.SelectionName);
                    }
                }
            });

            gearField.SubscribeOnChange(e =>
            {
                if(e is TextField.TextFieldChangeEvent changeEvent)
                {
                    var value = Regex.Replace(changeEvent.NewValue, @"[^0-9.]", ""); // Only allow integers and .
                    var i = value.IndexOf('.');
                    if(i != -1)
                    {
                        value = value.Substring(0, i + 1) + value.Substring(i).Replace(".", ""); // Only allow one .
                    }
                    gearField.SetValueWithoutNotify(value);
                }
            });

            gearField.SubscribeOnFocusLeave(e =>
            {
                if (e is TextField.TextFieldFocusLeaveEvent)
                {
                    if (gearField.Value != "")
                    {
                        assembly.GearReduction = double.Parse(gearField.Value);
                    }
                }
            });

            countField.SubscribeOnChange(e =>
            {
                if (e is TextField.TextFieldChangeEvent changeEvent)
                {
                    var value = Regex.Replace(changeEvent.NewValue, @"[^0-9]", ""); // Only allow integers
                    countField.SetValueWithoutNotify(value);
                }
            });

            countField.SubscribeOnFocusLeave(e =>
            {
                if (e is TextField.TextFieldFocusLeaveEvent leaveEvent)
                {
                    if (countField.Value != "")
                    {
                        if (uint.Parse(countField.Value) == 0)
                            countField.SetValueWithoutNotify(assembly.MotorCount.ToString());
                        else
                            assembly.MotorCount = uint.Parse(countField.Value);
                    }
                }
            });

            highlightButton.OnMouseEnter(() =>
            {
                HighlightButton();
            });

            highlightButton.OnMouseLeave(() =>
            {
                if(!IsHighlighted)
                    UnHighlightButton();
            });

            highlightButton.Subscribe(x =>
            {
                //toggle highlight
                if (!IsHighlighted)
                {
                    UnHighlightAllButtons();
                    HighlightButton(true);

                    Highlighter.HighlightJoint(assembly.Joint, assembly.Entity);
                }
            });
        }

        private void HighlightButton(bool stick = false)
        {
            if (stick)
                IsHighlighted = true;
            if(highlightButton != null)
                highlightButton.SetStyleProperty("background-color", "rgba(255, 255, 0, 1)");
        }

        private void UnHighlightButton()
        {
            IsHighlighted = false;
            if (highlightButton != null)
                highlightButton.SetStyleProperty("background-color", "rgba(255, 255, 0, 0)");
        }

        public static void UnHighlightAllButtons()
        {
            foreach(var i in jointItems)
            {
                i.UnHighlightButton();
            }
        }
    }

}
