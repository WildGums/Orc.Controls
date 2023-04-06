namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.PinnableToolTip))]
public class PinnableToolTip : FrameworkElement<PinnableToolTipModel, PinnableToolTipMap>
{
    public PinnableToolTip(AutomationElement element)
        : base(element)
    {
    }

    public bool IsPinned
    {
        get => Map.PinButton.IsToggled == true;
        set => Map.PinButton.IsToggled = value;
    }

    public Point Position
    {
        get => Element.Current.BoundingRectangle.TopLeft;
        set
        {
            var border = Map.GripBorder;
            border.DragAndDrop(value);
        }
    }

    public void Close()
    {
        this.MouseOut();
        Wait.UntilResponsive(100);

        Map.CloseButton?.Click();
    }
}
