namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AgeWizardPage : WizardPageBase
    {
        public AgeWizardPage()
        {
            Title = "Age";
            Description = "Specify the age of the person";
            IsOptional = true;
        }

        public string Age { get; set; }

        public override ISummaryItem GetSummary()
        {
            return new SummaryItem
            {
                Title = "Age",
                Summary = Age
            };
        }
    }
}
