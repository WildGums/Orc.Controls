namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using Catel.Data;
    using Catel.Fody;
    using Catel.MVVM;

    public class NumericTextBoxViewModel : ViewModelBase
    {
        public NumericTextBoxViewModel()
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
        [Expose(nameof(NumericTextBoxExampleModel.Value))]
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

            var value = Model?.Value;
            if (value.HasValue && value.Value == 0d)
            {
                validationResults.Add(FieldValidationResult.CreateError(nameof(Model.Value), "Demo validation for value of 0"));
            }
        }
    }

    public class NumericTextBoxExampleModel : ModelBase
    {
        public NumericTextBoxExampleModel()
        {
            Value = 42.42;
        }

        public double? Value { get; set; }
    }
}
