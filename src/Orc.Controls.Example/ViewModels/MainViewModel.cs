namespace Orc.Controls.Example.ViewModels
{
    using Catel.MVVM;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DeferValidationUntilFirstSaveCall = false;
        }

        public override string Title => "Orc.Controls example";
    }
}
