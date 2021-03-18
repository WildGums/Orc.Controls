namespace Orc.Controls.Controls.Callout.ViewModels
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.Controls.Controls.Callout.Views;

    public class CalloutViewModel : ViewModelBase
    {

        public CalloutViewModel()
        {
            ControlName = "";
            Description = "";
            Visible = false;
        }

        public string ControlName { get; set; } 

        public string Description { get; set; } 

        public bool Visible { get; set; }
    }
}
