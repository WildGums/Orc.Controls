namespace Orc.Controls;

using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using Automation;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Catel.Windows.Data;
using Catel.Windows.Interactivity;
using Microsoft.Extensions.Logging;
using Theming;

[TemplatePart(Name = "PART_WatermarkHost", Type = typeof(ContentPresenter))]
[TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
public partial class FilterBox : TextBox
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(FilterBox));

    private readonly Command _clearFilter;

    private Button? _clearButton;
    private ContentPresenter? _watermarkHost;
    private AutoCompletion? _autoCompletion;

    public FilterBox(IServiceProvider serviceProvider)
    {
        _clearFilter = new Command(serviceProvider, OnClearFilter, CanClearFilter);

        this.SubscribeToDependencyProperty(nameof(Padding), OnPaddingChanged);
    }

    #region Dependency Properties
    public bool AllowAutoCompletion
    {
        get { return (bool)GetValue(AllowAutoCompletionProperty); }
        set { SetValue(AllowAutoCompletionProperty, value); }
    }

    public static readonly DependencyProperty AllowAutoCompletionProperty = DependencyProperty.Register(
        nameof(AllowAutoCompletion), typeof(bool), typeof(FilterBox), 
        new PropertyMetadata(true, (sender, _) => ((FilterBox)sender).OnAllowAutoCompletionChanged()));

    public IEnumerable? FilterSource
    {
        get { return (IEnumerable?)GetValue(FilterSourceProperty); }
        set { SetValue(FilterSourceProperty, value); }
    }

    public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register(nameof(FilterSource), typeof(IEnumerable),
        typeof(FilterBox), new FrameworkPropertyMetadata(null, (sender, _) => ((FilterBox)sender).OnFilterSourceChanged()));

    public string? PropertyName
    {
        get { return (string?)GetValue(PropertyNameProperty); }
        set { SetValue(PropertyNameProperty, value); }
    }

    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string),
        typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            , (sender, _) => ((FilterBox)sender).OnPropertyNameChanged()));

    public string? Watermark
    {
        get { return (string?)GetValue(WatermarkProperty); }
        set { SetValue(WatermarkProperty, value); }
    }

    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string),
        typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    #endregion

    public event EventHandler<InitializingAutoCompletionServiceEventArgs>? InitializingAutoCompletionService;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _clearButton = GetTemplateChild("PART_ClearButton") as Button;
        if (_clearButton is null)
        {
            throw Logger.LogErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClearButton'");
        }
        _clearButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _clearFilter);

        _watermarkHost = GetTemplateChild("PART_WatermarkHost") as ContentPresenter;
        if (_watermarkHost is null)
        {
            throw Logger.LogErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_WatermarkHost'");
        }
        UpdateWaterMarkMargin();

        this.AttachBehavior<UpdateBindingOnTextChanged>();
        this.AttachBehavior<SelectTextOnFocus>();

        _autoCompletion = this.AttachBehavior<AutoCompletion>();

        UpdateAutoCompletion();

        var serviceEventArg = new InitializingAutoCompletionServiceEventArgs();
        OnInitializingAutoCompletionService(serviceEventArg);
        var autoCompletionService = serviceEventArg.AutoCompletionService;
        if (autoCompletionService is null)
        {
            return;
        }

        //Hack
        var autoCompletionServiceFieldInfo = _autoCompletion.GetType().GetField("_autoCompletionService", BindingFlags.Instance | BindingFlags.NonPublic);
        autoCompletionServiceFieldInfo?.SetValue(_autoCompletion, autoCompletionService);
    }

    private void OnPaddingChanged(object? sender, DependencyPropertyValueChangedEventArgs e)
    {
        UpdateWaterMarkMargin();
    }

    private void UpdateWaterMarkMargin()
    {
        if (_watermarkHost is null)
        {
            return;
        }

        var padding = Padding;
        var margin = new Thickness(padding.Left + 2d, padding.Top, padding.Right + 2d, padding.Bottom);
        _watermarkHost.SetCurrentValue(MarginProperty, margin);
    }

    private void UpdateAutoCompletion()
    {
        if (_autoCompletion is null)
        {
            return;
        }

        _autoCompletion.SetCurrentValue(AutoCompletion.PropertyNameProperty, PropertyName);
        _autoCompletion.SetCurrentValue(AutoCompletion.ItemsSourceProperty, FilterSource);
        _autoCompletion.SetCurrentValue(AutoCompletion.UseAutoCompletionServiceProperty, AllowAutoCompletion);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (!IsEnabled || e.Handled || (e.Key != Key.Escape || !CanClearFilter()))
        {
            return;
        }

        OnClearFilter();
        e.Handled = true;
    }

    private void OnClearFilter()
    {
        var allowAutoCompletion = AllowAutoCompletion;
        var filterSource = FilterSource;

        SetCurrentValue(AllowAutoCompletionProperty, false);
        SetCurrentValue(FilterSourceProperty, null);

        try
        {
            SetCurrentValue(TextProperty, string.Empty);
        }
        finally
        {
            SetCurrentValue(FilterSourceProperty, filterSource);
            SetCurrentValue(AllowAutoCompletionProperty, allowAutoCompletion);
        }
    }

    private bool CanClearFilter()
    {
        return !string.IsNullOrWhiteSpace(Text);
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        _clearFilter.RaiseCanExecuteChanged();

        base.OnTextChanged(e);
    }

    protected virtual void OnInitializingAutoCompletionService(InitializingAutoCompletionServiceEventArgs e)
    {
        InitializingAutoCompletionService?.Invoke(this, e);
    }

    private void OnAllowAutoCompletionChanged()
    {
        UpdateAutoCompletion();
    }

    private void OnFilterSourceChanged()
    {
        UpdateAutoCompletion();
    }

    private void OnPropertyNameChanged()
    {
        UpdateAutoCompletion();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new FilterBoxAutomationPeer(this);
    }
}
