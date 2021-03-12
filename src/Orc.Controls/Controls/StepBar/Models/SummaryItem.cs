namespace Orc.Controls.Controls.StepBar.Models
{
    public class SummaryItem : ISummaryItem
    {
        public IStepBarPage Page { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }
    }
}
