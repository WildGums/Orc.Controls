namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Catel.Data;
using Orc.Automation;

public class ValidationContextTypeTreeItem : ValidationContextTreeItemBase
{
    public ValidationContextTypeTreeItem(AutomationElement element)
        : base(element)
    {
    }

    public ValidationResultType Type
    {
        get
        {
            return _treeItem.Header.StartsWith(ValidationResultType.Error.ToString())
                ? ValidationResultType.Error
                : ValidationResultType.Warning;
        }
    }

    public IReadOnlyList<ValidationContextResultTreeItem> ResultNodes => _treeItem.ChildItems
        .Select(x => AutomationControlExtensions.As<ValidationContextResultTreeItem>(x))
        .ToList();
}