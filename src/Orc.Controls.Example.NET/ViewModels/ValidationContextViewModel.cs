// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;

    public class ValidationContextViewModel : ViewModelBase
    {
        #region Properties
        public IValidationContext ValidationContext { get; private set; }
        #endregion

        #region Methods
        protected override Task InitializeAsync()
        {
            var context = new ValidationContext();

            var result1 = BusinessRuleValidationResult.CreateErrorWithTag("Error1 message", "A");
            var result2 = BusinessRuleValidationResult.CreateWarningWithTag("Warning1 message", "B");
            var result3 = FieldValidationResult.CreateWarningWithTag("Property1", "Warning2 message", "C");

            var tag = new
            {
                Name = "A",
                Line = (int?)2
            };

            var result4 = BusinessRuleValidationResult.CreateErrorWithTag("Error2 message with object tag", tag);
            var result5 = BusinessRuleValidationResult.CreateErrorWithTag("Error3 message", "B");

            var result6 = BusinessRuleValidationResult.CreateError("Error3 message");
            var result7 = BusinessRuleValidationResult.CreateError("Error4 message");
            var result8 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning3 message", new
            {
                Name = "A",
                Line = 1
            });
            var result9 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning4 message", new
            {
                Name = "A",
                Line = 2
            });
            var result10 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning5 message", new
            {
                Name = "A",
                Line = 3,
                ColumnName = "ColA"
            });
            var result11 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning6 message", new
            {
                Name = "A",
                Line = 20,
                ColumnIndex = 2
            });
            var result12 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning7 message", new
            {
                Name = "A",
                Line = 12,
                ColumnName = "ColC",
                ColumnIndex = 3
            });
            var result13 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning8 message", new
            {
                Name = "A",
                Line = 10
            });
            var result14 = FieldValidationResult.CreateWarningWithTag("Property2", "Warning9 message", new
            {
                Name = "A",
                Line = 24
            });

            context.Add(result1);
            context.Add(result2);
            context.Add(result3);
            context.Add(result4);
            context.Add(result5);
            context.Add(result6);
            context.Add(result7);
            context.Add(result8);
            context.Add(result9);
            context.Add(result10);
            context.Add(result11);
            context.Add(result12);
            context.Add(result13);
            context.Add(result14);

            ValidationContext = context;

            return base.InitializeAsync();
        }
        #endregion
    }
}
