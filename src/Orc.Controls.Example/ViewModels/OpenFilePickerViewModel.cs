namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using Catel.Data;
using Catel.MVVM;

public partial class OpenFilePickerViewModel : ViewModelBase
{
    public OpenFilePickerViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Validate(true);
    }

    public string FileName { get; set; }

    protected override void ValidateFields(List<IFieldValidationResult> validationResults)
    {
        base.ValidateFields(validationResults);

        if (string.IsNullOrWhiteSpace(FileName))
        {
            validationResults.Add(FieldValidationResult.CreateError(nameof(FileName), "File name is required"));
        }
    }
}
