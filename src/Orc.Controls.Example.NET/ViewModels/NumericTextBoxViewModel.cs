// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatermarkViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using Catel.MVVM;

    public class NumericTextBoxViewModel : ViewModelBase
    {
        public NumericTextBoxViewModel()
        {
            IsNullValueAllowed = true;
            IsNegativeAllowed = true;
            IsDecimalAllowed = true;
            Format = string.Empty;
            MinValue = -50;
            MaxValue = 50;

            Value = 42.42;
        }

        public bool IsNullValueAllowed { get; set; }

        public bool IsNegativeAllowed { get; set; }

        public bool IsDecimalAllowed { get; set; }

        public string Format { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public double? Value { get; set; }
    }
}