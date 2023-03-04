#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class ValidationContextTreeMap : AutomationBase
{
    public ValidationContextTreeMap(AutomationElement element)
        : base(element)
    {
    }

    public Tree InnerTree => By.One<Tree>();
}
#nullable enable
