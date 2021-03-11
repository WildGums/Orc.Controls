namespace Orc.Controls.Controls.StepBar.Models.ViewModels
{
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;

    public class AgeWizardPageViewModel : WizardPageViewModelBase<AgeWizardPage>
    {
        public AgeWizardPageViewModel(AgeWizardPage wizardPage)
            : base(wizardPage)
        {
        }

        [ViewModelToModel]
        public string Age { get; set; }
    }
}
