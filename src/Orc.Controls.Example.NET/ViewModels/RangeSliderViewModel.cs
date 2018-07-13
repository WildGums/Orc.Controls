namespace Orc.Controls.Example.ViewModels
{
    using Catel.MVVM;

    public class RangeSliderViewModel : ViewModelBase
    {
        public RangeSliderViewModel()
        {
            MinValue = 0;
            MaxValue = 100;

            LowerValue = 20;
            UpperValue = 80;
        }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public double LowerValue { get; set; }

        public double UpperValue { get; set; }
    }
}
