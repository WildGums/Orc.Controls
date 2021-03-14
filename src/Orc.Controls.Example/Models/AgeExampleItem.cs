namespace Orc.Controls.Controls.StepBar.Models
{
    public class AgeExampleItem : StepBarPageBase
    {
        public AgeExampleItem()
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
