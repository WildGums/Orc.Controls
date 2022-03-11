namespace Orc.Controls.UI
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Automation;
    using Catel.Windows;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using Tests;
    using Border = Orc.Automation.Controls.Border;
    using FrameworkElement = System.Windows.FrameworkElement;
    using PinnableToolTip = PinnableToolTip;

    public class PinnableToolTipTestFacts : StyledControlTestFacts<PinnableToolTip>
    {
        [Target]
        public Automation.PinnableToolTip Target { get; set; }
        public PinnableToolTipServiceModel TargetSettings { get; set; }

        private Border _toolTipOwnerControl;

        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            var controlType = typeof(System.Windows.Controls.Border);

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

            Wait.UntilResponsive(1000);

            _toolTipOwnerControl = testHost.Part<Border>(new BorderFinder());

            var current = _toolTipOwnerControl.Current;

            current.Width = 50;
            current.Height = 50;
            current.Background = Brushes.Red;

            _toolTipOwnerControl.MouseOut();

            Wait.UntilResponsive(500);

            _toolTipOwnerControl.Execute<InitializePinnableToolTipsMethodRun>();

            Wait.UntilResponsive(500);

            TargetSettings = _toolTipOwnerControl.Model<PinnableToolTipServiceModel>();
            TargetSettings.ShowDuration = 2500;

            _toolTipOwnerControl.MouseHover();

            Wait.UntilResponsive(500);

            Target = GetPinnableToolTip(id);
        }

        [Test]
        public void CorrectlyResize()
        {
            var target = Target;
            var model = target.Current;

            model.GripColor = Colors.Red;
        }

        [Test]
        public void CorrectlyAutomaticallyCloseUnpinnedToolTip()
        {
            var target = Target;
            var toolTipSettings = TargetSettings;

            Wait.UntilResponsive(200);

            //Close opened tooltip
            target.Close();

            //And open again with test-controlled duration
            const int duration = 100;

            toolTipSettings.ShowDuration = duration;
            Wait.UntilResponsive();

            _toolTipOwnerControl.MouseHover();

            //Wait until it closed automatically
            Wait.UntilResponsive(duration);

            Assert.That(target.IsVisible(), Is.False);
        }

        [Test]
        public void CorrectlyAllowClose()
        {
            var target = Target;
            var model = target.Current;

            model.AllowCloseByUser = false;

            //Try close
            target.Close();

            Wait.UntilResponsive(200);

            //should stay visible
            Assert.That(target.IsVisible, Is.True);
        }

        [Test]
        public void CorrectlyPinToolTip()
        {
            var target = Target;
            var toolTipSettings = TargetSettings;

            const int showDuration = 1000;

            toolTipSettings.ShowDuration = showDuration;

            target.IsPinned = true;

            //Wait more than show duration
            Wait.UntilResponsive(showDuration + 500);

            //Because if pinned it should remain visible
            Assert.That(target.IsVisible(), Is.True);

            target.Close();
        }

        [TestCase(0d, 0d)]
        [TestCase(100d, 100d)]
        [TestCase(-100d, -100d)]
        public void CorrectlyMoveToolTip(double deltaX, double deltaY)
        {
            var target = Target;

            Wait.UntilResponsive();

            var positionBeforeDnD = target.Position;

            var expectedNewPosition = new Point(positionBeforeDnD.X + deltaX, positionBeforeDnD.Y + deltaY);

            //Move tooltip
            target.Position = expectedNewPosition;

            var positionAfterDnD = target.Position;
            
            target.IsPinned = true;

            Wait.UntilResponsive();

            Assert.That(positionAfterDnD, Is.EqualTo(expectedNewPosition));

            target.Close();
        }

        private Automation.PinnableToolTip GetPinnableToolTip(string targetId)
        {
            var hostWindow = Setup.MainWindow;

            //var hostWindow = element.GetHostWindow();
            var toolTips = hostWindow.FindAll<Automation.PinnableToolTip>() 
                           ?? Enumerable.Empty<Automation.PinnableToolTip>();
            
            var pinnableToolTip = toolTips.FirstOrDefault(x => Equals(x.Current.Owner, targetId));

            return pinnableToolTip;
        }
    }
    
    public class BorderFinder : IPartFinder
    {
        public FrameworkElement Find(FrameworkElement parent)
        {
            return parent.FindVisualDescendantByType<System.Windows.Controls.Border>();
        }
    }

    public class InitializePinnableToolTipsMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.NotSetValue;
            var pinnableToolTip = new PinnableToolTip
            {
                Name = "PinnableToolTipId",
                AllowCloseByUser = true,
                ResizeMode = ResizeMode.CanResize,
                MinWidth = 100,
                MinHeight = 100,
                HorizontalOffset = -12,
                VerticalOffset = -12,
            };

            var dataTemplate = new DataTemplate();
            var label = new FrameworkElementFactory(typeof(Label));
            label.SetValue(ContentControl.ContentProperty, "Test content");
            dataTemplate.VisualTree = label;
            pinnableToolTip.ContentTemplate = dataTemplate;

            owner.SetCurrentValue(PinnableToolTipService.ToolTipProperty, pinnableToolTip);

            result = AutomationValue.FromValue(true);

            return true;
        }
    }
}
