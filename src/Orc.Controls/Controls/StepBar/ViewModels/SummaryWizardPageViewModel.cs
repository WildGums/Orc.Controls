namespace Orc.Controls.Controls.StepBar.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orc.Controls.Controls.StepBar.Models;

    public class SummaryWizardPageViewModel : WizardPageViewModelBase<SummaryWizardPage>
    {
        public SummaryWizardPageViewModel(SummaryWizardPage wizardPage) 
            : base(wizardPage)
        {
        }

        public List<ISummaryItem> SummaryItems { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var wizard = Wizard;
            if (wizard is null)
            {
                return;
            }

            var items = new List<ISummaryItem>();

            foreach (var page in wizard.Pages)
            {
                // Skip pages that were not visited
                if (!page.IsVisited)
                {
                    continue;
                }

                var summary = page.GetSummary();
                if (summary != null)
                {
                    if (summary.Page is null)
                    {
                        summary.Page = page;
                    }

                    items.Add(summary);
                }
            }

            SummaryItems = items;
        }
    }
}
