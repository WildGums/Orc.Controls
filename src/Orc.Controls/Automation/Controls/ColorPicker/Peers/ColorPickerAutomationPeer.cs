namespace Orc.Controls.Automation;

using System.Windows.Media;
using System.Windows.Shapes;
using Catel.Windows;
using Orc.Automation;

public class ColorPickerAutomationPeer : AutomationControlPeerBase<Controls.ColorPicker>
{
    public ColorPickerAutomationPeer(Controls.ColorPicker owner)
        : base(owner)
    {
        owner.ColorChanged += OnColorChanged;
    }

    [AutomationMethod]
    public Color? GetRenderedElementColor()
    {
        var surface = Control.FindVisualDescendantByName("ColorSurface");
        if (surface is System.Windows.Controls.Control control)
        {
            return (control.Background as SolidColorBrush)?.Color;
        }

        if (surface is Shape shape)
        {
            return (shape.Fill as SolidColorBrush)?.Color;
        }

        return null;
    }

    private void OnColorChanged(object? sender, ColorChangedEventArgs e)
    {
        RaiseEvent(nameof(Controls.ColorPicker.ColorChanged), null);
    }
}
