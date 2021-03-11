namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;

    public class StepBarViewModel : ViewModelBase
    {
        public StepBarViewModel()
        {
            Pages.Add(ServiceLocator.Default.RegisterTypeAndInstantiate<AgeWizardPage>());
            Pages.Add(ServiceLocator.Default.RegisterTypeAndInstantiate<AgeWizardPage>());
            Pages.Add(ServiceLocator.Default.RegisterTypeAndInstantiate<AgeWizardPage>());
            Pages.Add(ServiceLocator.Default.RegisterTypeAndInstantiate<AgeWizardPage>());
        }

        public IList<IWizardPage> Pages { get; } = new List<IWizardPage>();
    }
}
