namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.Logging;
using Catel.MVVM;

public class Tag : ModelBase
{
    public bool IsEditing { get; set; }
    public string? Content { get; set; } = string.Empty;
}

[TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
//[TemplatePart(Name = "PART_DeleteTagButton", Type = typeof(Button))]
public class TagTextBox : Control
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private ListBox? _listBox;
    private HashSet<string> _removedFromSuggestedTags = new();
    //private Button? _deleteTagButton;

    private readonly ObservableCollection<Tag> _tags = new();

    public static RoutedCommand DeleteTag { get; } = new(nameof(DeleteTag), typeof(TagTextBox));


    public TagTextBox()
    {
        _tags.CollectionChanged += OnTagsCollectionChanged;

        CreateRoutedCommandBinding(DeleteTag, OnDeleteTag);
    }

    public IEnumerable<string>? Tags
    {
        get { return (IEnumerable<string>?)GetValue(TagsProperty); }
        set { SetValue(TagsProperty, value); }
    }

    public static readonly DependencyProperty TagsProperty = DependencyProperty.Register(
        nameof(Tags), typeof(IEnumerable<string>), typeof(TagTextBox), new PropertyMetadata(default(IEnumerable<string>),
            (sender, args) => ((TagTextBox)sender).OnTagsChanged(args)));

    public IEnumerable<string>? SuggestedTags
    {
        get { return (IEnumerable<string>?)GetValue(SuggestedTagsProperty); }
        set { SetValue(SuggestedTagsProperty, value); }
    }

    public static readonly DependencyProperty SuggestedTagsProperty = DependencyProperty.Register(
        nameof(SuggestedTags), typeof(IEnumerable<string>), typeof(TagTextBox), new PropertyMetadata(Array.Empty<string>(),
            (sender, args) => ((TagTextBox)sender).OnSuggestedTagsChanged(args)));

    public IReadOnlyCollection<string> SelectedTags
    {
        get { return (IReadOnlyCollection<string>)GetValue(SelectedTagsProperty); }
        set { SetValue(SelectedTagsProperty, value); }
    }

    public static readonly DependencyProperty SelectedTagsProperty = DependencyProperty.Register(
        nameof(SelectedTags), typeof(IReadOnlyCollection<string>), typeof(TagTextBox), new PropertyMetadata(Array.Empty<string>(),
            (sender, args) => ((TagTextBox)sender).OnSelectedTagsChanged(args)));

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

    private void OnSelectedTagsChanged(DependencyPropertyChangedEventArgs args)
    {
    }


    private void OnSuggestedTagsChanged(DependencyPropertyChangedEventArgs args)
    {
        //no flag
        _removedFromSuggestedTags.Clear();
    }

    private void OnTagsChanged(DependencyPropertyChangedEventArgs args)
    {
        UpdateListBox();
    }

    private void OnTagsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_listBox is null)
        {
            return;
        }

        var allTags = Tags;
        if (allTags is null)
        {
            return;
        }

        var suggestedTags = SuggestedTags;
        if (suggestedTags is null)
        {
            return;
        }

        var possibleTagsSuggestions = suggestedTags.Except(_tags.Select(x => x.Content));
        SetCurrentValue(SuggestedTagsProperty, possibleTagsSuggestions);
    }

    private void OnDeleteTag(object param)
    {
        var deleteTag = (string)param;

        var tagToDelete = _tags.FirstOrDefault(x => x.Content.EqualsIgnoreCase(deleteTag));
        if (tagToDelete is null)
        {
            return;
        }

        _tags.Remove(tagToDelete);
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
            
            return;
        }

        if (e.Key == Key.Delete)
        {
            if (_listBox?.SelectedItem is Tag selectedTag)
            {
                _tags.Remove(selectedTag);
            }
        }
    }

    private void CreateRoutedCommandBinding(RoutedCommand routedCommand, Action<object> executeAction, Func<object, bool>? canExecute = null)
    {
        var routedCommandBinding = new CommandBinding
        {
            Command = routedCommand
        };
        routedCommandBinding.Executed += (_, args) => executeAction.Invoke(args.Parameter);

        if (canExecute is not null)
        {
            routedCommandBinding.CanExecute += (_, args) => args.CanExecute = canExecute.Invoke(args.Parameter);
        }

        CommandBindings.Add(routedCommandBinding);
    }
}
