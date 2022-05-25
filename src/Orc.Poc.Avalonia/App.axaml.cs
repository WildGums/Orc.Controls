using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Orc.Poc.Avalonia.ViewModels;
using Orc.Poc.Avalonia.Views;

namespace Orc.Poc.Avalonia
{
    using Catel.IoC;

    public partial class App : Application
    {
        public override void Initialize()
        {
#pragma warning disable IDISP001
            var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001
            serviceLocator.RegisterType<IMyModelProvider, MyModelProvider>();

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
