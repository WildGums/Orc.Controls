namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.ColorPicker))]
    public class ColorPicker : FrameworkElement<ColorPickerModel>
    {
        private ToggleButton ToggleDropDown => By.ControlType(ControlType.Button).One<ToggleButton>();

        public ColorPicker(AutomationElement element)
            : base(element)
        {
        }

        //View
        public Color Color
        {
            get
            {
                var rect = Element.Current.BoundingRectangle;
                var color = PixelHelper.GetColorAt(rect.GetClickablePoint());

                return color;
            }

            set
            {
                var colorBoard = OpenColorBoard();
                if (colorBoard is null)
                {
                    return;
                }

                colorBoard.ArgbColor = value;

                colorBoard.Apply();
            }
        }

        public ColorBoard OpenColorBoard()
        {
            var windowHost = Element.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
            if (windowHost is null)
            {
                return null;
            }

            ToggleDropDown.Toggle();

            Wait.UntilResponsive();

            var colorPopup = windowHost.Find(className: "Popup", controlType: ControlType.Window);
            var colorBoard = colorPopup?.Find(className: typeof(Controls.ColorBoard).FullName);
            if (colorBoard is null)
            {
                return null;
            }

            return new ColorBoard(colorBoard);
        }

#pragma warning disable CS0067
        public event EventHandler<EventArgs> ColorChanged;
#pragma warning restore CS0067
    }
}
