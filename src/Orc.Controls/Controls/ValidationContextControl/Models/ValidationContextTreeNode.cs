namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Catel.Collections;
using Catel.Data;

public class ValidationContextTreeNode : ChildAwareModelBase, IValidationContextTreeNode, IComparable
{
    protected ValidationContextTreeNode(bool isExpanded)
    {
        Children = new ObservableCollection<ValidationContextTreeNode>();
        IsExpanded = isExpanded;
    }
    
    public ObservableCollection<ValidationContextTreeNode> Children { get; }
    public string? DisplayName { get; protected set; }
    public bool IsExpanded { get; set; }
    public bool IsVisible { get; set; }

    public ValidationResultType? ResultType { get; set; }
    IEnumerable<IValidationContextTreeNode> IValidationContextTreeNode.Children => Children.OfType<IValidationContextTreeNode>();

    public int CompareTo(object? obj)
    {
        return obj is ValidationContextTreeNode validationContextTreeNode 
            ? CompareTo(validationContextTreeNode)
            : 1;
    }
    
    public void ApplyFilter(bool showErrors, bool showWarnings, string filter)
    {
        foreach (var validationContextTreeNode in Children)
        {
            validationContextTreeNode.ApplyFilter(showErrors, showWarnings, filter);
        }

        if (Children.Any(x => x.IsVisible))
        {
            IsVisible = true;
            return;
        }

        var isVisible = ResultType is null || showErrors && ResultType == ValidationResultType.Error 
                                           || showWarnings && ResultType == ValidationResultType.Warning;

        if (!isVisible)
        {
            IsVisible = false;
            return;
        }

        if (string.IsNullOrEmpty(filter))
        {
            IsVisible = true;
            return;
        }

        var culture = CultureInfo.InvariantCulture;
        IsVisible = culture.CompareInfo.IndexOf(DisplayName ?? string.Empty, filter, CompareOptions.IgnoreCase) >= 0;
    }

    public virtual int CompareTo(ValidationContextTreeNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return CultureInfo.InstalledUICulture.CompareInfo.Compare(DisplayName, node.DisplayName);
    }
}
