namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorPickerAutomationElement : CommandAutomationElement
    {
        public ColorPickerAutomationElement(AutomationElement element)
            : base(element)
        {
        }

        public Color Color
        {
            get => (Color)GetValue(nameof(ColorPicker.Color));
            set => SetValue(nameof(ColorPicker.Color), value);
        }
    }
}
