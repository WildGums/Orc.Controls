﻿namespace Orc.Controls;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Catel;
using Catel.Data;

public class ValidationResultTagNode : ValidationContextTreeNode
{
    public ValidationResultTagNode(string tagName, bool isExpanded)
        : base(isExpanded)
    {
        TagName = tagName;
    }

    public string TagName { get; }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (string.Equals(e.PropertyName, nameof(DisplayName)))
        {
            return;
        }

        UpdateDisplayName();
    }

    protected override void OnPropertyObjectCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateDisplayName();

        base.OnPropertyObjectCollectionChanged(sender, e);
    }

    private void UpdateDisplayName()
    {
        var children = Children;// ?? Enumerable.Empty<ValidationResultTypeNode>();
        
        var errorCount = children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Error)
            .SelectMany(x => x.Children).Count();

        var warningCount = children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Warning)
            .SelectMany(x => x.Children).Count();

        DisplayName = $"{TagName} ({LanguageHelper.GetString("Controls_ValidationContextControl_Errors")}: {errorCount}, {LanguageHelper.GetString("Controls_ValidationContextControl_Warnings")}: {warningCount})";
    }

    public override int CompareTo(ValidationContextTreeNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var misc = LanguageHelper.GetRequiredString("Controls_ValidationContextControl_Misc");

        var validationResultTagNode = (ValidationResultTagNode)node;
        if (TagName.EqualsIgnoreCase(misc) && !validationResultTagNode.TagName.EqualsIgnoreCase(misc))
        {
            return 1;
        }

        if (!TagName.EqualsIgnoreCase(misc) && validationResultTagNode.TagName.EqualsIgnoreCase(misc))
        {
            return -1;
        }

        return CultureInfo.InstalledUICulture.CompareInfo.Compare(TagName, validationResultTagNode.TagName);
    }
}
