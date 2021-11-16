namespace Orc.Controls.Controls
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using Automation;
    using Catel.Logging;
    using Orc.Controls.Automation;

    [TemplatePart(Name = "PART_HostGrid", Type = typeof(Grid))]
    public class TestHost : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private const string TestedControlAutomationId = "TestedControlId";

        private Grid _hostGrid;

        public override void OnApplyTemplate()
        {
            _hostGrid = GetTemplateChild("PART_HostGrid") as Grid;
            if (_hostGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HostGrid'");
            }
        }

        public string PutControl(string fullName)
        {
            var controlType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(x => string.Equals(x.FullName, fullName));

            if (controlType is null)
            {
                return $"Error: Can't find control Type {fullName}";
            }

            if (Activator.CreateInstance(controlType) is not FrameworkElement control)
            {
                return $"Error: Can't instantiate control Type {controlType}";
            }

            control.SetCurrentValue(AutomationProperties.AutomationIdProperty, TestedControlAutomationId);

            _hostGrid.Children.Add(control);

            return TestedControlAutomationId;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TestHostAutomationPeer(this);
        }
    }
}
