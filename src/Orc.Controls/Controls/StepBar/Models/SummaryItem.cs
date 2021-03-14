namespace Orc.Controls.Controls.StepBar.Models
{
    public class SummaryItem : ISummaryItem
    {
        public IStepBarItem Page { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }
    }
}
