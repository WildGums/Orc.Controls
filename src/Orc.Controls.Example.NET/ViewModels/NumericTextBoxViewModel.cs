// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatermarkViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
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
