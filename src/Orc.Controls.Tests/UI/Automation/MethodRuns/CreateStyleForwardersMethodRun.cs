namespace Orc.Controls.Tests
{
    using System.Windows;
    using Orc.Automation;
    using Theming;

    public class CreateStyleForwardersMethodRun : NamedAutomationMethodRun
    {
        public override string Name => nameof(CreateStyleForwardersMethodRun);

        public CreateStyleForwardersMethodRun()
        {
            
        }

        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            StyleHelper.CreateStyleForwardersForDefaultStyles();

            result = AutomationValue.FromValue(10);

            return true;
        }
    }
}
