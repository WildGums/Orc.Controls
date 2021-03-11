namespace Orc.Controls.Controls.StepBar.Models
{
    using Catel;
    using Catel.Services;

    public class SummaryWizardPage : WizardPageBase
    {
        public SummaryWizardPage(ILanguageService languageService)
        {
            Argument.IsNotNull(() => languageService);

            Title = languageService.GetString("Wizard_SummaryTitle");
            Description = languageService.GetString("Wizard_SummaryDescription");
        }
    }
}
