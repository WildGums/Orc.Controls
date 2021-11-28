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
            get => (Color)GetValue(nameof(ColorPicker.Color));
            set => SetValue(nameof(ColorPicker.Color), value);
        }

        public Color CurrentColor
        {
            get => (Color)GetValue(nameof(ColorPicker.CurrentColor));
            set => SetValue(nameof(ColorPicker.CurrentColor), value);
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(nameof(ColorPicker.IsDropDownOpen));
            set => SetValue(nameof(ColorPicker.IsDropDownOpen), value);
        }

        public ColorPickerUserActions Simulate => _colorPickerUserActions ??= this.CreateUserActions<ColorPickerUserActions>();
    }
}
