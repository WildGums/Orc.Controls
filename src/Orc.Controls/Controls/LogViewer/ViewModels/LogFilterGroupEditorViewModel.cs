// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.Controls.Models;

    public class LogFilterGroupEditorViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public LogFilterGroupEditorViewModel(IMessageService messageService) : this(new LogFilterGroup(), messageService)
        {
        }

        public LogFilterGroupEditorViewModel(LogFilterGroup logFilterGroup, IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
            LogFilterGroup = logFilterGroup;

            AddOrSaveCommand = new Command(OnAddOrSaveCommandExecute, OnAddOrSaveCommandCanExecute);
            EditCommand = new Command(OnEditCommandExecute, OnEditCommandCanExecute);
            CancelCommand = new Command(OnCancelCommandExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecute, OnRemoveCommandCanExecute);
        }

        private bool OnRemoveCommandCanExecute()
        {
            return SelectedLogFilter != null;
        }

        private async Task OnRemoveCommandExecute()
        {
            var result = await _messageService.ShowAsync("Are you sure?", button: MessageButton.YesNo, icon: MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                LogFilterGroup.LogFilters.Remove(SelectedLogFilter);
                SelectedLogFilter = null;
            }

        }

        public TaskCommand RemoveCommand { get; set; }

        [Model]
        public LogFilterGroup LogFilterGroup { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public string Name { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public ObservableCollection<LogFilter> LogFilters { get; set; }

        public string AddOrSaveCommandText { get; private set; }

        [Model]
        public LogFilter LogFilter { get; set; }

        [ViewModelToModel(nameof(LogFilter), "Name")]
        public string LogFilterName { get; set; }

        [ViewModelToModel(nameof(LogFilter))]
        public string ExpressionValue { get; set; }

        public LogFilter SelectedLogFilter { get; set; }

        public override string Title => "Log Filter Group Editor";

        public Command AddOrSaveCommand { get; }

        public Command EditCommand { get; }

        public Command CancelCommand { get; }

        private bool OnAddOrSaveCommandCanExecute()
        {
            return !string.IsNullOrWhiteSpace(LogFilterName) && !string.IsNullOrWhiteSpace(ExpressionValue);
        }

        protected override void OnValidating(IValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationContext.Add(FieldValidationResult.CreateError(nameof(Name), "'Name' for the LogFilterGroup is required"));
            }

            if (LogFilters == null || LogFilters.Count == 0)
            {
                validationContext.Add(FieldValidationResult.CreateError(nameof(LogFilters), "At least one LogFilter for the is required"));
            }
        }

        private void OnCancelCommandExecute()
        {
            (LogFilter as IEditableObject).CancelEdit();
            InitializeLogFilter();
        }

        private bool OnEditCommandCanExecute()
        {
            return SelectedLogFilter != null;
        }

        private void OnEditCommandExecute()
        {
            LogFilter = SelectedLogFilter;
            AddOrSaveCommandText = "Save";
            ((IEditableObject) LogFilter).BeginEdit();
        }

        private void OnAddOrSaveCommandExecute()
        {
            if (LogFilters.IndexOf(LogFilter) == -1)
            {
                LogFilters.Add(LogFilter);
            }
            else
            {
                (LogFilter as IEditableObject).EndEdit();
            }

            InitializeLogFilter();
        }

        protected override async Task InitializeAsync()
        {
            InitializeLogFilter();
        }

        private void InitializeLogFilter()
        {
            SelectedLogFilter = null;
            LogFilter = new LogFilter();
            AddOrSaveCommandText = "Add";
            ((IEditableObject) LogFilter).BeginEdit();
        }
    }
}
