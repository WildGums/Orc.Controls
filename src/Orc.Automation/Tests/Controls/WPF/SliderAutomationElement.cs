﻿namespace Orc.Automation.Tests
{
    using System.Windows.Automation;

    //https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-support-for-the-slider-control-type
    public sealed class SliderAutomationElement : ProtectedByControlTypeAutomationElementBase
    {
        public SliderAutomationElement(AutomationElement element) 
            : base(element, ControlType.Slider)
        {
        }

        public double SmallChange
        {
            get => Element.GetRangeSmallChange();
        }

        public double LargeChange
        {
            get => Element.GetRangeLargeChange();
        }

        public double Minimum
        {
            get => Element.GetRangeMinimum();
        }

        public double Maximum
        {
            get => Element.GetRangeMaximum();
        }

        public double Value
        {
            get => Element.GetValue<double>();
            set => Element.SetValue(value);
        }
    }
}
