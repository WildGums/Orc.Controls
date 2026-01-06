namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.MVVM;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override string Title => "Orc.Controls example";
    }
}
