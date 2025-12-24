namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;

public class LogFilterEditorViewModel : FeaturedViewModelBase
{
    private readonly ILanguageService _languageService;

    public LogFilterEditorViewModel(LogFilter logFilter, IServiceProvider serviceProvider,
        ILanguageService languageService)
        : base(serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(logFilter);

        _languageService = languageService;

        ValidateUsingDataAnnotations = false;

        ExpressionTypes = new ObservableCollection<LogFilterExpressionType>(Enum<LogFilterExpressionType>.GetValues());
        Actions = new ObservableCollection<LogFilterAction>(Enum<LogFilterAction>.GetValues());
        Targets = new ObservableCollection<LogFilterTarget>(Enum<LogFilterTarget>.GetValues());

        LogFilter = logFilter;
    }

    public ObservableCollection<LogFilterAction> Actions { get; }
    public ObservableCollection<LogFilterExpressionType> ExpressionTypes { get; }
    public ObservableCollection<LogFilterTarget> Targets { get; }

    [Model]
    public LogFilter? LogFilter { get; set; }

    [ViewModelToModel]
    public string? Name { get; set; }

    [ViewModelToModel]
    public LogFilterExpressionType ExpressionType { get; set; }

    [ViewModelToModel]
    public string? ExpressionValue { get; set; }

    [ViewModelToModel]
    public LogFilterAction Action { get; set; }

    [ViewModelToModel]
    public LogFilterTarget Target { get; set; }

    protected override void ValidateFields(List<IFieldValidationResult> validationResults)
    {
        base.ValidateFields(validationResults);

        if (string.IsNullOrWhiteSpace(Name))
        {
            validationResults.Add(FieldValidationResult.CreateError(nameof(Name), _languageService.GetRequiredString(nameof(Properties.Resources.Controls_LogViewer_LogFilterEditor_NameIsRequired))));
        }

        if (string.IsNullOrWhiteSpace(ExpressionValue))
        {
            validationResults.Add(FieldValidationResult.CreateError(nameof(ExpressionValue), _languageService.GetRequiredString(nameof(Properties.Resources.Controls_LogViewer_LogFilterEditor_ExpressionValueIsRequired))));
        }
    }
}
