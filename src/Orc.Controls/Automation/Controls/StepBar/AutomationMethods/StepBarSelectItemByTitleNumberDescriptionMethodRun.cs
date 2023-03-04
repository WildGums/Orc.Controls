#nullable disable
namespace Orc.Controls.Automation;

using System.Linq;
using System.Windows;
using Orc.Automation;

public class StepBarSelectItemByTitleNumberDescriptionMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(true);

        if (owner is not Controls.StepBar stepBar)
        {
            return false;
        }

        var parameters = method.Parameters;
        var title = parameters[0].ExtractValue() as string;
        var number = (int) parameters[1].ExtractValue();
        var description = parameters[2].ExtractValue() as string;

        stepBar.SelectedItem = stepBar.Items?.FirstOrDefault(x => Equals(x.Title, title) && Equals(x.Number, number) && Equals(x.Description, description));

        return true;
    }
}
#nullable enable
