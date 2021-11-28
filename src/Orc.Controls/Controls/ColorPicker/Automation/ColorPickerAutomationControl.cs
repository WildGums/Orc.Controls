namespace Orc.Controls.Automation
{
    using System.Runtime.CompilerServices;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorPickerAutomationControl : RunMethodAutomationControl
    {
        private ColorPickerUserActions _colorPickerUserActions;

        public ColorPickerAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public Color Color
        {
            get => (Color)GetApiPropertyValue(nameof(ColorPicker.Color));
            set => SetApiPropertyValue(nameof(ColorPicker.Color), value);
        }

        public Color CurrentColor
        {
            get => (Color)GetApiPropertyValue(nameof(ColorPicker.CurrentColor));
            set => SetApiPropertyValue(nameof(ColorPicker.CurrentColor), value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetApiPropertyValue(nameof(ColorPicker.IsDropDownOpen));
            set => SetApiPropertyValue(nameof(ColorPicker.IsDropDownOpen), value);
        }

        public ColorPickerUserActions Simulate => _colorPickerUserActions ??= this.CreateUserActions<ColorPickerUserActions>();
    }
}
