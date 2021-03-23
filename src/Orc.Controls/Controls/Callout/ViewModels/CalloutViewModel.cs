namespace Orc.Controls.Controls.Callout.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;
    using Orc.Controls.Controls.Callout.Views;

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
