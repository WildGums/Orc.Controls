namespace Orc.Controls.Controls.StepBar.Models
{
    public interface ISummaryItem
    {
        IStepBarPage Page { get; set; }

        string Title { get; set; }
        string Summary { get; set; }
    }
}
