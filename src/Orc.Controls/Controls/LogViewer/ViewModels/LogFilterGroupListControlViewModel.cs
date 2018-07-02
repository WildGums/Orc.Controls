// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupListControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Controls.Models;
    using Orc.Controls.Services;
    using Path = Catel.IO.Path;

    public class LogFilterGroupListControlViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IApplicationFilterGroupService _applicationFilterGroupService;

        private readonly IMessageService _messageService;

        private readonly IUIVisualizerService _uIVisualizerService;

        public LogFilterGroupListControlViewModel(IApplicationFilterGroupService applicationFilterGroupService, IMessageService messageService, IUIVisualizerService uIVisualizerService)
        {
            Argument.IsNotNull(() => applicationFilterGroupService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => uIVisualizerService);

            _applicationFilterGroupService = applicationFilterGroupService;
            _messageService = messageService;
            _uIVisualizerService = uIVisualizerService;

            AddCommand = new TaskCommand(OnAddCommandExecute);
            EditCommand = new TaskCommand(OnEditCommandExecute, OnEditCommandCanExecute);
            EnableOrDisableCommand = new Command(OnEnableOrDisableCommandExecute, OnEnableOrDisableCommandCanExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecute, OnRemoveCommandCanExecute);
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

        private async Task OnRemoveCommandExecute()
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

        private async Task OnAddCommandExecute()
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

        private async Task OnEditCommandExecute()
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
            FilterGroups = new FastObservableCollection<LogFilterGroup>(_applicationFilterGroupService.Load());
        }

        private void SaveFilterGroups()
        {
            _applicationFilterGroupService.Save(FilterGroups);
        }
    }
}
