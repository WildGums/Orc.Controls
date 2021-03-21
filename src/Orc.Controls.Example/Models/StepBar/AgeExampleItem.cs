namespace Orc.Controls.Example.Models.StepBar
{
    using Orc.Controls;

    public class AgeExampleItem : StepBarItemBase
    {
        public AgeExampleItem()
        {
            Title = "Age";
            Description = "Specify the age of the person";
            IsOptional = true;
        }

        public string Age { get; set; }
    }
}
