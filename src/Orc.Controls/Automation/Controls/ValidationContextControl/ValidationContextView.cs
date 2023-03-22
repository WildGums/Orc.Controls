#nullable disable
namespace Orc.Controls.Automation;

using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation;
using Catel.Data;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(ValidationContextView))]
public class ValidationContextView : FrameworkElement<ValidationContextViewModel, ValidationContextViewMap>,
    IEnumerable<ValidationContextTagTreeItem>
{
    public ValidationContextView(AutomationElement element) 
        : base(element)
    {
    }

    public IReadOnlyList<ValidationContextTagTreeItem> TabItems => Map.Tree.TagNodes;

    //public string GetContents()
    //{
    //    return Map.Tree.GetContents();
    //}

    public IReadOnlyList<string> GetValidationItems(string tag, ValidationResultType type)
    {
        return Map.Tree.GetValidationItems(tag, type);
    }

    public bool IsFilterVisible => Map.FilterBox.IsVisible();

    public string Filter
    {
        get => IsFilterVisible ? Map.FilterBox.Text : string.Empty;
        set
        {
            if (!IsFilterVisible)
            {
                return;
            }

            Map.FilterBox.Text = value;
        }
    }

    public bool IsExpanded
    {
        get => Map.CollapseAllButton.IsVisible();
        set
        {
            if (value)
            {
                Map.ExpandAllButton?.Click();
            }
            else
            {
                Map.CollapseAllButton?.Click();
            }
        }
    }

    public bool IsErrorsVisible
    {
        get => Map.ShowErrorsButton.IsToggled;
        set => Map.ShowErrorsButton.IsToggled = value;
    }

    public bool IsWarningsVisible
    {
        get => Map.ShowWarningButton.IsToggled;
        set => Map.ShowWarningButton.IsToggled = value;
    }

    public IEnumerator<ValidationContextTagTreeItem> GetEnumerator()
    {
        return TabItems.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
#nullable enable
