namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM;

    public class WizardPageViewModelLocator : ViewModelLocator, IWizardPageViewModelLocator
    {
        public WizardPageViewModelLocator()
        {
            NamingConventions.Add("[CURRENT].ViewModels.[VW]PageViewModel");
            NamingConventions.Add("[CURRENT].ViewModels.[VW]ViewModel");
        }
    }
}
