namespace Orc.Controls.Automation;

using System.Windows.Automation;

public class ValidationContextResultTreeItem : ValidationContextTreeItemBase
{
    public ValidationContextResultTreeItem(AutomationElement element)
        : base(element)
    {
    }

    public string Message => _treeItem.Header;
}