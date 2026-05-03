#nullable disable
namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Input;
using Orc.Automation;

public class SetNumericTextBoxValueRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(true);

        if (owner is not Orc.Controls.NumericTextBox numericTextBox)
        {
            return false;
        }

        var beenFocused = numericTextBox.IsKeyboardFocused;

        //We should move focus of the control
        //TODO:Vladimir:this is actually bug in control, but it prevent us from testing other controls based on this one, so... would be fixed lately
        if (beenFocused)
        {
            numericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        var value = (double?)method.Parameters[0].ExtractValue();
        numericTextBox.SetCurrentValue(Orc.Controls.NumericTextBox.ValueProperty, value);

        if (beenFocused)
        {
            Keyboard.Focus(numericTextBox);
        }

        return true;
    }
}
#nullable enable
