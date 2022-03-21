namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class LinkLabelMap : AutomationBase
    {
        public LinkLabelMap(AutomationElement element) 
            : base(element)
        {
            
        }

        public Hyperlink Hyperlink => By.One<Hyperlink>();
        public Text Text => Element.GetRawChild(0).As<Text>();
    }
}
