namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;

public class ValidationContextTagTreeItem : ValidationContextTreeItemBase
{
    public ValidationContextTagTreeItem(AutomationElement element) 
        : base(element)
    {
    }

    public string Tag
    {
        get
        {
            var header = _treeItem.Header ?? string.Empty;
            return header[..(header.IndexOf('(') - 1)];
        }
    } 

    public IReadOnlyList<ValidationContextTypeTreeItem> TypeNodes => _treeItem.ChildItems
        .Select(x => x.As<ValidationContextTypeTreeItem>())
        .ToList();
}
