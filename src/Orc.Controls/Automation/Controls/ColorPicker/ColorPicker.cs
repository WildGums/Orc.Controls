#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Windows.Automation;
using System.Windows.Media;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.ColorPicker))]
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
            //TODO:Vladimir: re-think
            var color = Access.Execute(nameof(ColorPickerAutomationPeer.GetRenderedElementColor)) as Color?;

            return color ?? Current.Color;
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
        var colorBoard = colorPopup?.Find(className: "Orc.Controls.ColorBoard[ActiveAutomationControl]");
        return colorBoard is not null 
            ? new ColorBoard(colorBoard) 
            : null;
    }

#pragma warning disable CS0067
    public event EventHandler<EventArgs> ColorChanged;
#pragma warning restore CS0067
}
#nullable enable
