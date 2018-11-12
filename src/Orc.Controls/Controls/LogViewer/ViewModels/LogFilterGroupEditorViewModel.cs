// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupEditorViewModel.cs" company="WildGums">
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
        #region Fields
        private readonly IMessageService _messageService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
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
        }
        #endregion

        #region Properties
        public override string Title => LanguageHelper.GetString("Controls_LogViewer_LogFilterGroupEditor_Title");

        public TaskCommand AddCommand { get; }
        public TaskCommand EditCommand { get; }
        public TaskCommand RemoveCommand { get; }

        [Model]
        public LogFilterGroup LogFilterGroup { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public ObservableCollection<LogFilter> LogFilters { get; set; }

        [ViewModelToModel(nameof(LogFilterGroup))]
        public string Name { get; set; }

        public LogFilter SelectedLogFilter { get; set; }
        #endregion

        #region Methods
        protected override void OnValidating(IValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                validationContext.Add(FieldValidationResult.CreateError(nameof(Name),
                    LanguageHelper.GetString("Controls_LogViewer_LogFilterGroupEditor_NameForTheLogFilterGroupIsRequired")));
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
            var result = await _messageService.ShowAsync(LanguageHelper.GetString("Controls_LogViewer_AreYouSure"),
                button: MessageButton.YesNo, icon: MessageImage.Warning);
            if (result == MessageResult.Yes)
            {
                LogFilterGroup.LogFilters.Remove(SelectedLogFilter);
                SelectedLogFilter = null;

                Validate(true);
            }
        }
        #endregion
    }
}
