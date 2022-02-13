namespace Orc.Controls.Automation
{
    using Orc.Automation;

    public class NumericTextBoxAutomationPeer : ControlRunMethodAutomationPeerBase<Controls.NumericTextBox>
    {
        public NumericTextBoxAutomationPeer(Controls.NumericTextBox owner)
            : base(owner)
        {
        }

        //public override object GetPattern(PatternInterface patternInterface)
        //{
        //    if (patternInterface is PatternInterface.Text)
        //    {
        //        return this;
        //    }

        //    return base.GetPattern(patternInterface);
        //}
    }
}
