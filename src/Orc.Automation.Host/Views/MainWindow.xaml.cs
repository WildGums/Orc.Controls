namespace Orc.Automation.Host.Views
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Windows;
    using System.Windows.Baml2006;
    using System.Xaml;
    using Theming;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CanCloseUsingEscape = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls\net5.0-windows\Orc.Controls.dll");

            TestHost.LoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");

            TestHost.PutControl("Orc.Controls.ColorLegend");
        }
    }
}
