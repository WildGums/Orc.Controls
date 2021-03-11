namespace Orc.Controls.Controls.StepBar.Models
{    public interface ISummaryItem
    {
        IWizardPage Page { get; set; }

        string Title { get; set; }
        string Summary { get; set; }
    }
}
