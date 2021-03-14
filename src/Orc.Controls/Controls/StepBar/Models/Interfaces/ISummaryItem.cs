namespace Orc.Controls.Controls.StepBar.Models
{
    public interface ISummaryItem
    {
        IStepBarItem Page { get; set; }

        string Title { get; set; }
        string Summary { get; set; }
    }
}
