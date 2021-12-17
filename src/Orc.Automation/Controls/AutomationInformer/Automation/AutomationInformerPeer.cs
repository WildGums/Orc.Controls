namespace Orc.Automation
{
    using System.Windows;
    using System.Windows.Automation;
    using Catel.Windows;

    public class AutomationInformerPeer : ControlRunMethodAutomationPeerBase<AutomationInformer>
    {
        public AutomationInformerPeer(AutomationInformer owner)
            : base(owner)
        {
        }

        [AutomationMethod]
        public object GetTargetPropertyValue(string propertyName, string targetId)
        {
            var targetControl = Control.FindVisualDescendant(x => Equals((x as FrameworkElement)?.GetValue(AutomationProperties.AutomationIdProperty), targetId));
            if (targetControl is null)
            {
                return AutomationValue.NotSetValue;
            }

            if (DependencyPropertyAutomationHelper.TryGetDependencyPropertyValue(targetControl, propertyName, out var propertyValue))
            {
                return propertyValue;
            }

            return AutomationValue.NotSetValue;
        }

        [AutomationMethod]
        public void SetTargetPropertyValue(string propertyName, string targetId, object value)
        {
            var targetControl = Control.FindVisualDescendant(x => Equals((x as FrameworkElement)?.GetValue(AutomationProperties.AutomationIdProperty), targetId));
            if (targetControl is null)
            {
                return;
            }

            DependencyPropertyAutomationHelper.SetDependencyPropertyValue(targetControl, propertyName, value);
        }
    }
}
