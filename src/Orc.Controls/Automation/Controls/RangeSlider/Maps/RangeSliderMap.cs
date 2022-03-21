namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class RangeSliderMap : AutomationBase
    {
        public RangeSliderMap(AutomationElement element) 
            : base(element)
        {
            
        }

        public Slider LowerSlider => By.Id("PART_LowerSlider").One<Slider>();
        public Slider UpperSlider => By.Id("PART_UpperSlider").One<Slider>();
    }
}
