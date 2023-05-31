namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Catel.Collections;
using Catel.Data;
using Catel.Logging;

public class Tag : ModelBase
{
    public bool IsEditing { get; set; }
    public string? Content { get; set; } = string.Empty;
}

[TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
public class TagTextBox : Control
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private ListBox? _listBox;

    private readonly ObservableCollection<Tag> _tags = new();

    public IEnumerable<string>? Tags
    {
        get { return (IEnumerable<string>?)GetValue(TagsProperty); }
        set { SetValue(TagsProperty, value); }
    }

    public static readonly DependencyProperty TagsProperty = DependencyProperty.Register(
        nameof(Tags), typeof(IEnumerable<string>), typeof(TagTextBox), new PropertyMetadata(default(IEnumerable<string>),
            (sender, args) => ((TagTextBox)sender).OnTagsChanged(args)));

    public IReadOnlyCollection<string> SelectedTags
    {
        get { return (IReadOnlyCollection<string>)GetValue(SelectedTagsProperty); }
        set { SetValue(SelectedTagsProperty, value); }
    }

    public static readonly DependencyProperty SelectedTagsProperty = DependencyProperty.Register(
        nameof(SelectedTags), typeof(IReadOnlyCollection<string>), typeof(TagTextBox), new PropertyMetadata(Array.Empty<string>(),
            (sender, args) => ((TagTextBox)sender).OnSelectedTagsChanged(args)));

    private void OnSelectedTagsChanged(DependencyPropertyChangedEventArgs args)
    {
    }

    private void OnTagsChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdateListBox();
    }

    public override void OnApplyTemplate()
    {
        _listBox = GetTemplateChild("PART_ListBox") as ListBox;
        if (_listBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ListBox'");
        }
        _listBox?.SetCurrentValue(ItemsControl.ItemsSourceProperty, _tags);

        UpdateListBox();
    }

    private void UpdateListBox()
    {
        _tags.Clear();
        _tags.AddRange(Tags?.Select(x => new Tag
        {
            Content = x
        }) ?? Enumerable.Empty<Tag>());

        _tags.Add(new Tag
        {
            IsEditing = true
        });
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _tags.ForEach(x => x.IsEditing = false);
            _tags.Add(new Tag { IsEditing = true });
        }
    }
}
