// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupListViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;

    public class LogFilterGroupListViewModel : ViewModelBase
    {
        #region Fields
        private readonly IApplicationLogFilterGroupService _applicationLogFilterGroupService;
        private readonly IMessageService _messageService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public LogFilterGroupListViewModel(IApplicationLogFilterGroupService applicationLogFilterGroupService, IMessageService messageService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => applicationLogFilterGroupService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => uiVisualizerService);

            _applicationLogFilterGroupService = applicationLogFilterGroupService;
            _messageService = messageService;
            _uiVisualizerService = uiVisualizerService;

            FilterGroups = new FastObservableCollection<LogFilterGroup>();
            SelectedFilterGroups = new ObservableCollection<LogFilterGroup>();

            AddCommand = new TaskCommand(OnAddCommandExecuteAsync);
            EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
        }
        #endregion

        #region Properties
        public ObservableCollection<LogFilterGroup> FilterGroups { get; private set; }
        public ObservableCollection<LogFilterGroup> SelectedFilterGroups { get; private set; }
        public LogFilterGroup SelectedFilterGroup { get; set; }

        public TaskCommand AddCommand { get; set; }
        public TaskCommand EditCommand { get; set; }
        public TaskCommand RemoveCommand { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await LoadFilterGroupsAsync();
        }

        private async Task LoadFilterGroupsAsync()
        {
            FilterGroups.ReplaceRange(await _applicationLogFilterGroupService.LoadAsync());
        }

        private async Task SaveFilterGroupsAsync()
        {
            await _applicationLogFilterGroupService.SaveAsync(FilterGroups);
        }

        private async Task OnAddCommandExecuteAsync()
        {
            var logFilterGroup = new LogFilterGroup();
            if (await _uiVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(logFilterGroup) ?? false)
            {
                FilterGroups.Add(logFilterGroup);
                await SaveFilterGroupsAsync();

                Updated.SafeInvoke(this);
            }
        }

        private bool OnEditCommandCanExecute()
        {
            return SelectedFilterGroup != null;
        }

        private async Task OnEditCommandExecuteAsync()
        {
            if (await _uiVisualizerService.ShowDialogAsync<LogFilterGroupEditorViewModel>(SelectedFilterGroup) ?? false)
            {
                await SaveFilterGroupsAsync();

                Updated.SafeInvoke(this);
            }
        }

        private bool OnRemoveCommandCanExecute()
        {
            return SelectedFilterGroup != null;
        }

        private async Task OnRemoveCommandExecuteAsync()
        {
            var result = await _messageService.ShowAsync(LanguageHelper.GetString("Controls_LogViewer_AreYouSure"),
                button: MessageButton.YesNo, icon: MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                FilterGroups.Remove(SelectedFilterGroup);
                await SaveFilterGroupsAsync();
                SelectedFilterGroup = null;

                Updated.SafeInvoke(this);
            }
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> Updated;
        #endregion
    }
}
