namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class NumericUpDownMap : AutomationBase
    {
        public NumericUpDownMap(AutomationElement element)
            : base(element)
        {
        }

        public Border Chrome => By.Name().Part<Border>();

        public SpinButton SpinButton => By.One<SpinButton>();

        public Edit Edit => By.One<Edit>();
    }
}
