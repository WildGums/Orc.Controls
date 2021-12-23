namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ColorPickerView : AutomationBase
    {
        protected virtual ToggleButton ToggleDropDown => By.ControlType(ControlType.Button).One<ToggleButton>();

        public ColorPickerView(ColorPicker control)
            : base(control.Element)
        {
        }

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

                var colorBoardView = colorBoard.View;
                colorBoardView.ArgbColor = value;

                colorBoard.View.Apply();
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
    }
}
