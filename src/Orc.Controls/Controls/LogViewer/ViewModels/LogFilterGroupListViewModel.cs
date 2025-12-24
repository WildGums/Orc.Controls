namespace Orc.Controls;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Catel;
using Catel.Collections;
using Catel.MVVM;
using Catel.Services;

public class LogFilterGroupListViewModel : ViewModelBase
{
    private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;
    private readonly IMessageService _messageService;
    private readonly IUIVisualizerService _uiVisualizerService;
    private readonly ILanguageService _languageService;
    private readonly IDispatcherService _dispatcherService;

    public LogFilterGroupListViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService, 
        IMessageService messageService, IUIVisualizerService uiVisualizerService, IServiceProvider serviceProvider,
        ILanguageService languageService, IDispatcherService dispatcherService)
    {
        _applicationLogFilterGroupService = applicationLogFilterGroupService;
        _messageService = messageService;
        _uiVisualizerService = uiVisualizerService;
        _languageService = languageService;
        _dispatcherService = dispatcherService;

        ValidateUsingDataAnnotations = false;

        FilterGroups = new FastObservableCollection<LogFilterGroup>(_dispatcherService);
        SelectedFilterGroups = new ObservableCollection<LogFilterGroup>();

        AddCommand = new TaskCommand(serviceProvider, OnAddCommandExecuteAsync);
        EditCommand = new TaskCommand(serviceProvider, OnEditCommandExecuteAsync, OnEditCommandCanExecute);
        RemoveCommand = new TaskCommand(serviceProvider, OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
    }

    public ObservableCollection<LogFilterGroup> FilterGroups { get; private set; }
    public ObservableCollection<LogFilterGroup> SelectedFilterGroups { get; private set; }
    public LogFilterGroup? SelectedFilterGroup { get; set; }

    public TaskCommand AddCommand { get; set; }
    public TaskCommand EditCommand { get; set; }
    public TaskCommand RemoveCommand { get; set; }

    public event EventHandler<EventArgs>? Updated;

    protected override async Task InitializeAsync()
    {
        await LoadFilterGroupsAsync();
    }

    private async Task LoadFilterGroupsAsync()
    {
        var filterGroups = await _applicationLogFilterGroupService.LoadAsync();

        FilterGroups.ReplaceRange(filterGroups/*.Where(x => !x.IsRuntime)*/.OrderBy(x => x.Name));
    }

    private async Task SaveFilterGroupsAsync()
    {
        await _applicationLogFilterGroupService.SaveAsync(FilterGroups);
    }

    private async Task OnAddCommandExecuteAsync()
    {
        var logFilterGroup = new LogFilterGroup();

        var result = await _uiVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(logFilterGroup);
        if (result.DialogResult ?? false)
        {
            FilterGroups.Add(logFilterGroup);
            await SaveFilterGroupsAsync();

            Updated?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool OnEditCommandCanExecute()
    {
        var selectedFilterGroup = SelectedFilterGroup;
        if (selectedFilterGroup is null)
        {
            return false;
        }

        return !selectedFilterGroup.IsRuntime;
    }

    private async Task OnEditCommandExecuteAsync()
    {
        var result = await _uiVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(SelectedFilterGroup);
        if (result.DialogResult ?? false)
        {
            await SaveFilterGroupsAsync();

            Updated?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool OnRemoveCommandCanExecute()
    {
        var selectedFilterGroup = SelectedFilterGroup;
        if (selectedFilterGroup is null)
        {
            return false;
        }

        return !selectedFilterGroup.IsRuntime;
    }

    private async Task OnRemoveCommandExecuteAsync()
    {
        var result = await _messageService.ShowAsync(_languageService.GetRequiredString(nameof(Properties.Resources.Controls_LogViewer_AreYouSure)),
            button: MessageButton.YesNo, icon: MessageImage.Warning);
        if (result == MessageResult.Yes)
        {
            var selectedFilterGroup = SelectedFilterGroup;
            if (selectedFilterGroup is not null)
            {
                FilterGroups.Remove(selectedFilterGroup);
                await SaveFilterGroupsAsync();
                SelectedFilterGroup = null;
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}
