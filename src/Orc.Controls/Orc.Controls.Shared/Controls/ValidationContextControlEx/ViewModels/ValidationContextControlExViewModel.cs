// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextControlExViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel.Data;
    using Catel.MVVM;

    internal class ValidationContextControlExViewModel : ViewModelBase
    {
        public ValidationContextControlExViewModel()
        {
            Copy = new Command(OnCopyExecute);
            Open = new Command(OnOpenExecute);
        }

        public ValidationContext ValidationContext { get; set; }
        public bool ShowErrors { get; set; } = true;
        public bool ShowWarnings { get; set; } = true;
        public int ErrorsCount { get; private set; }
        public int WarningsCount { get; private set; }
        public List<IValidationResult> ValidationResults { get; private set; }
        public bool ShowFilterBox { get; set; }
        public string Filter { get; set; }

        #region Commands
        public Command Copy { get; private set; }

        private void OnCopyExecute()
        {
            
        }

        public Command Open { get; private set; }

        private void OnOpenExecute()
        {
            
        }
        #endregion

        private void OnValidationContextChanged()
        {
            var validationContext = ValidationContext;
            ErrorsCount = validationContext.GetErrorCount();
            WarningsCount = validationContext.GetWarningCount();

            ValidationResults = validationContext.GetValidations();
        }
    }
}