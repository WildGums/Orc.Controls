namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using Catel.Data;
using Catel.Fody;
using Catel.MVVM;

public partial class NumericTextBoxViewModel : FeaturedViewModelBase
{
    public NumericTextBoxViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        IsNullValueAllowed = true;
        IsNegativeAllowed = true;
        IsDecimalAllowed = true;
        Format = "F0";
        MinValue = -50;
        MaxValue = 50;

        Model = new NumericTextBoxExampleModel();
    }

    [Model]
    [Expose(nameof(NumericTextBoxExampleModel.IntValue))]
    [Expose(nameof(NumericTextBoxExampleModel.DoubleValue))]
    public NumericTextBoxExampleModel Model { get; private set; }

    public bool IsNullValueAllowed { get; set; }

    public bool IsNegativeAllowed { get; set; }

    public bool IsDecimalAllowed { get; set; }

    public string Format { get; set; }

    public double MinValue { get; set; }

    public double MaxValue { get; set; }

    protected override void ValidateFields(List<IFieldValidationResult> validationResults)
    {
        base.ValidateFields(validationResults);

        var value = Model?.DoubleValue;
        if (value.HasValue && value.Value == 0d)
        {
            validationResults.Add(FieldValidationResult.CreateError(nameof(Model.DoubleValue), "Demo validation for value of 0"));
        }
    }
}

public class NumericTextBoxExampleModel : ModelBase
{
    public NumericTextBoxExampleModel()
    {
        IntValue = 42;
        DoubleValue = 42.42;
    }

    public int? IntValue { get; set; }

    public double? DoubleValue { get; set; }
}
