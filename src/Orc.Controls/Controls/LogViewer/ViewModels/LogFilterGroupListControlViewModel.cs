// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupListControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;

    public class LogFilterGroupListControlViewModel : ViewModelBase
    {
        private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;

        private readonly IMessageService _messageService;

        private readonly IUIVisualizerService _uIVisualizerService;

        public LogFilterGroupListControlViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService, IMessageService messageService, IUIVisualizerService uIVisualizerService)
        {
            Argument.IsNotNull(() => applicationLogFilterGroupService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => uIVisualizerService);

            _applicationLogFilterGroupService = applicationLogFilterGroupService;
            _messageService = messageService;
            _uIVisualizerService = uIVisualizerService;

            AddCommand = new TaskCommand(OnAddCommandExecuteAsync);
            EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
            EnableOrDisableCommand = new Command(OnEnableOrDisableCommandExecute, OnEnableOrDisableCommandCanExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
        }

        public ObservableCollection<LogFilterGroup> FilterGroups { get; set; }

        public LogFilterGroup SelectedFilterGroup { get; set; }

        public TaskCommand AddCommand { get; set; }

        public TaskCommand EditCommand { get; set; }

        public TaskCommand RemoveCommand { get; set; }

        public Command EnableOrDisableCommand { get; set; }

        public string EnableOrDisableCommandText
        {
            get
            {
                if (SelectedFilterGroup != null)
                {
                    return SelectedFilterGroup.IsEnabled ? "Disable" : "Enable";
                }

                return string.Empty;
            }
        }

        public bool IsFilterGroupSelected => SelectedFilterGroup != null;

        private bool OnRemoveCommandCanExecute()
        {
            return SelectedFilterGroup != null;
        }

        private async Task OnRemoveCommandExecuteAsync()
        {
            var result = await _messageService.ShowAsync("Are you sure?", button:MessageButton.YesNo, icon:MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                FilterGroups.Remove(SelectedFilterGroup);
                SaveFilterGroups();
                SelectedFilterGroup = null;
            }
        }

        private void OnEnableOrDisableCommandExecute()
        {
            SelectedFilterGroup.IsEnabled = !SelectedFilterGroup.IsEnabled;
            SaveFilterGroups();
            RaisePropertyChanged(() => EnableOrDisableCommandText);
        }

        private async Task OnAddCommandExecuteAsync()
        {
            var logFilterGroup = new LogFilterGroup();
            var result = await _uIVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(logFilterGroup, null);
            if ((bool)result)
            {
                FilterGroups.Add(logFilterGroup);
                SaveFilterGroups();
            }
        }

        private bool OnEnableOrDisableCommandCanExecute()
        {
            return IsFilterGroupSelected;
        }

        private bool OnEditCommandCanExecute()
        {
            return IsFilterGroupSelected;
        }

        private async Task OnEditCommandExecuteAsync()
        {
            if (SelectedFilterGroup is IEditableObject editableObject)
            {
                editableObject.BeginEdit();
                var result = await _uIVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(SelectedFilterGroup, null);
                if ((bool)result)
                {
                    editableObject.EndEdit();
                    SaveFilterGroups();
                }
                else
                {
                    editableObject.CancelEdit();
                }
            }
        }

        protected override async Task InitializeAsync()
        {
            LoadFilterGroups();
        }

        private void LoadFilterGroups()
        {
            FilterGroups = new FastObservableCollection<LogFilterGroup>(_applicationLogFilterGroupService.LoadAsync().GetAwaiter().GetResult());
        }

        private void SaveFilterGroups()
        {
            _applicationLogFilterGroupService.SaveAsync(FilterGroups);
        }
    }
}
