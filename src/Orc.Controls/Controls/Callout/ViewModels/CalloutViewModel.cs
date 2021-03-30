namespace Orc.Controls
{
    using System.Windows;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {
        public CalloutViewModel()
        {
        }

        public bool IsOpen { get; set; }

        public UIElement PlacementTarget { get; set; }

        public string ControlName { get; set; } 

        public string Description { get; set; } 

        public object InnerContent { get; set; }
    }
}
