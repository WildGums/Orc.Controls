namespace Orc.Controls
{
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

        public LogFilterGroupListViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService, IMessageService messageService, IUIVisualizerService uiVisualizerService)
        {
            ArgumentNullException.ThrowIfNull(applicationLogFilterGroupService);
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);

            _applicationLogFilterGroupService = applicationLogFilterGroupService;
            _messageService = messageService;
            _uiVisualizerService = uiVisualizerService;

            FilterGroups = new FastObservableCollection<LogFilterGroup>();
            SelectedFilterGroups = new ObservableCollection<LogFilterGroup>();

            AddCommand = new TaskCommand(OnAddCommandExecuteAsync);
            EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
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

            if (selectedFilterGroup.IsRuntime)
            {
                return false;
            }

            return true;
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

            if (selectedFilterGroup.IsRuntime)
            {
                return false;
            }

            return true;
        }

        private async Task OnRemoveCommandExecuteAsync()
        {
            var result = await _messageService.ShowAsync(LanguageHelper.GetRequiredString("Controls_LogViewer_AreYouSure"),
                button: MessageButton.YesNo, icon: MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                FilterGroups.Remove(SelectedFilterGroup);
                await SaveFilterGroupsAsync();
                SelectedFilterGroup = null;

                Updated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
