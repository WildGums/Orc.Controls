// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    public class LogFilterGroupEditorViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public LogFilterGroupEditorViewModel(IMessageService messageService)
            : this(new LogFilterGroup(), messageService)
        {
        }

        public LogFilterGroupEditorViewModel(LogFilterGroup logFilterGroup, IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
            LogFilterGroup = logFilterGroup;

            AddOrSaveCommand = new TaskCommand(OnAddOrSaveCommandExecuteAsync, OnAddOrSaveCommandCanExecute);
            EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
            CancelCommand = new TaskCommand(OnCancelCommandExecuteAsync);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);
        }

        public TaskCommand AddOrSaveCommand { get; }

        public string AddOrSaveCommandText { get; private set; }

        public TaskCommand CancelCommand { get; }

        public TaskCommand EditCommand { get; }

        [ViewModelToModel(nameof(LogFilter))]
        public string ExpressionValue { get; set; }

        [Model]
        public LogFilter LogFilter { get; set; }

        [Model]
        public LogFilterGroup LogFilterGroup { get; set; }

        [ViewModelToModel(nameof(LogFilter), "Name")]
        public string LogFilterName { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public ObservableCollection<LogFilter> LogFilters { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public string Name { get; set; }

        public TaskCommand RemoveCommand { get; set; }

        public LogFilter SelectedLogFilter { get; set; }

        public override string Title => "Log Filter Group Editor";

        protected override async Task InitializeAsync()
        {
            InitializeLogFilter();
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

        private void InitializeLogFilter()
        {
            SelectedLogFilter = null;
            LogFilter = new LogFilter();
            AddOrSaveCommandText = "Add";
            ((IEditableObject)LogFilter).BeginEdit();
        }

        private bool OnAddOrSaveCommandCanExecute()
        {
            return !string.IsNullOrWhiteSpace(LogFilterName) && !string.IsNullOrWhiteSpace(ExpressionValue);
        }

        private async Task OnAddOrSaveCommandExecuteAsync()
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

        private async Task OnCancelCommandExecuteAsync()
        {
            (LogFilter as IEditableObject).CancelEdit();
            InitializeLogFilter();
        }

        private bool OnEditCommandCanExecute()
        {
            return SelectedLogFilter != null;
        }

        private async Task OnEditCommandExecuteAsync()
        {
            LogFilter = SelectedLogFilter;
            AddOrSaveCommandText = "Save";
            ((IEditableObject)LogFilter).BeginEdit();
        }

        private bool OnRemoveCommandCanExecute()
        {
            return SelectedLogFilter != null;
        }

        private async Task OnRemoveCommandExecuteAsync()
        {
            var result = await _messageService.ShowAsync("Are you sure?", button: MessageButton.YesNo, icon: MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                LogFilterGroup.LogFilters.Remove(SelectedLogFilter);
                SelectedLogFilter = null;
            }
        }
    }
}
