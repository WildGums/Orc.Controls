namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class PinnableToolTipMap : AutomationBase
    {
        public PinnableToolTipMap(AutomationElement element) 
            : base(element)
        {
            
        }

        public Border GripBorder => By.Name("PART_DragGrip").Part<Border>();
        public ToggleButton PinButton => By.Id("PART_PinButton").One<ToggleButton>();
        public Button CloseButton => By.Id("PART_CloseButton").One<Button>();
    }
}
