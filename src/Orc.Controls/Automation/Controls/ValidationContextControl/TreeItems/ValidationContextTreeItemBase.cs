namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public abstract class ValidationContextTreeItemBase : FrameworkElement
{
    protected readonly TreeItem _treeItem;

    protected ValidationContextTreeItemBase(AutomationElement element) 
        : base(element)
    {
        _treeItem = element.As<TreeItem>();
    }

    public bool IsExpanded
    {
        get => _treeItem.IsExpanded;
        set => _treeItem.IsExpanded = value;
    }


}