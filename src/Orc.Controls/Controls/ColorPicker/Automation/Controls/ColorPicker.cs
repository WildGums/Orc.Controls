namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using AutomationEventArgs = Orc.Automation.AutomationEventArgs;

    [AutomatedControl(Class = typeof(Controls.ColorPicker))]
    public class ColorPicker : FrameworkElement
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

        protected override void OnEvent(object sender, AutomationEventArgs args)
        {
            if (args.EventName == nameof(Controls.ColorPicker.ColorChanged))
            {
                ColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> ColorChanged;
    }
}
