namespace Orc.Controls;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Catel;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;

public class LogFilterGroupEditorViewModel : ViewModelBase
{
    private readonly IMessageService _messageService;
    private readonly IUIVisualizerService _uiVisualizerService;

    public LogFilterGroupEditorViewModel(LogFilterGroup logFilterGroup, IMessageService messageService,
        IUIVisualizerService uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(logFilterGroup);
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(uiVisualizerService);

        _messageService = messageService;
        _uiVisualizerService = uiVisualizerService;
        LogFilterGroup = logFilterGroup;
        LogFilters = new ObservableCollection<LogFilter>();

        AddCommand = new TaskCommand(OnAddCommandExecuteAsync);
        EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
        RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
    }

    public override string Title => LanguageHelper.GetRequiredString("Controls_LogViewer_LogFilterGroupEditor_Title");

    public TaskCommand AddCommand { get; }
    public TaskCommand EditCommand { get; }
    public TaskCommand RemoveCommand { get; }

    [Model]
    public LogFilterGroup LogFilterGroup { get; set; }

    [ViewModelToModel(nameof(LogFilterGroup))]
    public ObservableCollection<LogFilter> LogFilters { get; set; }

    [ViewModelToModel(nameof(LogFilterGroup))]
    public string? Name { get; set; }

    public LogFilter? SelectedLogFilter { get; set; }

    protected override void OnValidating(IValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            validationContext.Add(FieldValidationResult.CreateError(nameof(Name),
                LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_LogViewer_LogFilterGroupEditor_NameForTheLogFilterGroupIsRequired))));
        }
    }

    private async Task OnAddCommandExecuteAsync()
    {
        var logFilter = new LogFilter();

        var result = await _uiVisualizerService.ShowDialogAsync<LogFilterEditorViewModel>(logFilter);
        if (result.DialogResult ?? false)
        {
            LogFilters.Add(logFilter);
            SelectedLogFilter = logFilter;

            Validate(true);
        }
    }

    private bool OnEditCommandCanExecute()
    {
        return SelectedLogFilter is not null;
    }

    private async Task OnEditCommandExecuteAsync()
    {
        var logFilter = SelectedLogFilter;
        var result = await _uiVisualizerService.ShowDialogAsync<LogFilterEditorViewModel>(logFilter);
        if (result.DialogResult ?? false)
        {
            // No action required
        }
    }

    private bool OnRemoveCommandCanExecute()
    {
        return SelectedLogFilter is not null;
    }

    private async Task OnRemoveCommandExecuteAsync()
    {
        var result = await _messageService.ShowAsync(LanguageHelper.GetRequiredString(nameof(Properties.Resources.Controls_LogViewer_AreYouSure)),
            button: MessageButton.YesNo, icon: MessageImage.Warning);
        if (result == MessageResult.Yes)
        {
            var selectedLogFilter = SelectedLogFilter;
            if (selectedLogFilter is not null)
            {
                LogFilterGroup.LogFilters.Remove(selectedLogFilter);
                SelectedLogFilter = null;
            }
            
            Validate(true);
        }
    }
}
