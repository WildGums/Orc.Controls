// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
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
            Argument.IsNotNull(() => logFilterGroup);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => uiVisualizerService);

            _messageService = messageService;
            _uiVisualizerService = uiVisualizerService;
            LogFilterGroup = logFilterGroup;

            AddCommand = new TaskCommand(OnAddCommandExecuteAsync);
            EditCommand = new TaskCommand(OnEditCommandExecuteAsync, OnEditCommandCanExecute);
            RemoveCommand = new TaskCommand(OnRemoveCommandExecuteAsync, OnRemoveCommandCanExecute);

            Title = "Log Filter Group Editor";
        }

        public TaskCommand AddCommand { get; }

        public TaskCommand EditCommand { get; }

        public TaskCommand RemoveCommand { get; set; }

        [Model]
        public LogFilterGroup LogFilterGroup { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public ObservableCollection<LogFilter> LogFilters { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public string Name { get; set; }

        public LogFilter SelectedLogFilter { get; set; }

        protected override void OnValidating(IValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationContext.Add(FieldValidationResult.CreateError(nameof(Name), "'Name' for the LogFilterGroup is required"));
            }
        }
        
        private async Task OnAddCommandExecuteAsync()
        {
            var logFilter = new LogFilter();
            if (await _uiVisualizerService.ShowDialogAsync<LogFilterEditorViewModel>(logFilter) ?? false)
            {
                LogFilters.Add(logFilter);
                SelectedLogFilter = logFilter;

                Validate(true);
            }
        }

        private bool OnEditCommandCanExecute()
        {
            return SelectedLogFilter != null;
        }

        private async Task OnEditCommandExecuteAsync()
        {
            var logFilter = SelectedLogFilter;
            if (await _uiVisualizerService.ShowDialogAsync<LogFilterEditorViewModel>(logFilter) ?? false)
            {
                // No action required
            }
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

                Validate(true);
            }
        }
    }
}
