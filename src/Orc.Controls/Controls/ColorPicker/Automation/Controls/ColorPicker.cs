namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    [AutomatedControl(Class = typeof(Controls.ColorPicker))]
    public class ColorPicker : AutomationControl
    {
        public ColorPicker(AutomationElement element)
            : base(element)
        {
            View = new ColorPickerView(this);
        }

        public ColorPickerView View { get; }

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
    }
}
