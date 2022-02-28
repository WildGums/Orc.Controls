namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel.Windows;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public partial class StaggeredPanelFacts
    {
        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            var controlType = typeof(System.Windows.Controls.ItemsControl);

            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\DiffEngine.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalUtilities.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalTests.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.dll");
            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Automation.Tests.dll");

            testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            var result = testHost.TryLoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            testHost.TryLoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");

            return testHost.TryLoadControlWithForwarders(controlType, out testedControlAutomationId);
        }

        protected override void InitializeTarget(string id)
        {
            var window = Setup.MainWindow;

            var testHost = window.Find<TestHostAutomationControl>(className: typeof(TestHost).FullName);
            if (testHost is null)
            {
                return;
            }

            testHost.Execute<InitializeStaggeredPanelMethodRun>();

            Wait.UntilResponsive(1000);

            var staggeredPanel = testHost.Find(className: typeof(StaggeredPanel).FullName);
            if (staggeredPanel is null)
            {
                Assert.Fail("Can't find staggered panel");
            }
            
            staggeredPanel.InitializeControlMap(this);
        }

        protected void InitializeItems(List<StaggeredPanelTestItem> itemSource)
        {
            var window = Setup.MainWindow;

            var testHost = window.Find<TestHostAutomationControl>(className: typeof(TestHost).FullName);

            testHost?.Execute<InitializeItemsMethodRun>(itemSource);
        }
    }

    public class InitializeItemsMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(System.Windows.FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.NotSetValue;

            if (owner is not TestHost testHost)
            {
                return false;
            }

            var itemsControl = testHost.FindVisualDescendantByType<System.Windows.Controls.ItemsControl>();
            if (itemsControl is null)
            {
                return false;
            }

            var itemSource = method.Parameters.FirstOrDefault()?.ExtractValue() as IEnumerable;

            itemsControl.ItemsSource = itemSource;

            result = AutomationValue.FromValue(true);

            return true;
        }
    }
    
    public class InitializeStaggeredPanelMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(System.Windows.FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.NotSetValue;

            if (owner is not TestHost testHost)
            {
                return false;
            }

            var itemsControl = testHost.FindVisualDescendantByType<System.Windows.Controls.ItemsControl>();
            if (itemsControl is null)
            {
                return false;
            }

            itemsControl.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StaggeredPanel)));

            var dataTemplate = new DataTemplate(typeof(StaggeredPanelTestItem));

            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            //textBlock.SetBinding(TextBlock.WidthProperty, new Binding(nameof(StaggeredPanelTestItem.Width)));
            //textBlock.SetBinding(TextBlock.HeightProperty, new Binding(nameof(StaggeredPanelTestItem.Height)));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(nameof(StaggeredPanelTestItem.Content)));
            
            dataTemplate.VisualTree = textBlock;

            itemsControl.ItemTemplate = dataTemplate;

            result = AutomationValue.FromValue(true);

            return true;
        }
    }

    public class StaggeredPanelTestItem
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Content { get; set; }
    }
}
