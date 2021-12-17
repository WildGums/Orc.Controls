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
            var testHost = TestHost;

            var peer = new TestHostAutomationPeer(testHost);
            peer.SetValue(File.ReadAllText(@"C:\Temps\temps.txt"));

            peer.Invoke();

            testHost.LoadAssembly("C:\\Source\\Orc.Controls\\output\\Debug\\Orc.Controls.Tests\\net6.0-windows\\Orc.Controls.dll");

            //testHost.LoadUnmanaged(@"C:\Source\Gum.Controls\output\Debug\Gum.Controls.Tests\net5.0-windows\runtimes\win-x64\native\libSkiaSharp.dll");
            //testHost.LoadUnmanaged(@"C:\Source\Gum.Controls\output\Debug\Gum.Controls.Tests\net5.0-windows\runtimes\win-x64\native\sni.dll");

            //testHost.LoadAssembly("C:\\Source\\Gum.Controls\\output\\Debug\\Gum.Controls.Tests\\net5.0-windows\\Gum.Drawing.dll");
            //testHost.LoadAssembly("C:\\Source\\Gum.Controls\\output\\Debug\\Gum.Controls.Tests\\net5.0-windows\\Gum.Controls.Tests.dll");



            ////testHost.LoadAssembly("C:\\Source\\Gum.Controls\\output\\Debug\\Gum.Controls.Tests\\net5.0-windows\\Gum.Controls.Tests.dll");

            //testHost.LoadResources("pack://application:,,,/Gum.Controls.DataGrid;component/Themes/Generic.xaml");

            //var testedControlAutomationId = testHost.PutControl("Gum.Controls.DataGrid");


            //TestHost.LoadAssembly(@"C:\Source\Gum.Controls\src\Gum.Controls.Example\bin\Debug\net5.0-windows\Gum.Drawing.dll");
            //TestHost.LoadUnmanaged(@"C:\Source\Gum.Controls\src\Gum.Controls.Example\bin\Debug\net5.0-windows\runtimes\win-x64\native\libSkiaSharp.dll");
            //TestHost.LoadAssembly("C:\\Source\\Gum.Controls\\output\\Debug\\Gum.Controls.Tests\\net5.0-windows\\Gum.Controls.Tests.dll");

            //TestHost.LoadResources("pack://application:,,,/Gum.Controls.DataGrid;component/Themes/Generic.xaml");

            //TestHost.PutControl("Gum.Controls.DataGrid");
        }
    }
}
