namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorPickerAutomationControl : CustomAutomationControl
    {
        public ColorPickerAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public Color Color
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        public Color CurrentColor
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        public bool IsDropDownOpen
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public ColorPickerUserActions Simulate => Simulate<ColorPickerUserActions>();
    }
}
