namespace Orc.Controls
{
    using System.Windows;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {

        public CalloutViewModel()
        {
            
        }

        public UIElement PlacementTarget { get; set; }

        public string ControlName { get; set; } 

        public string Description { get; set; } 

        public Visibility Visible { get; set; }
    }
}
