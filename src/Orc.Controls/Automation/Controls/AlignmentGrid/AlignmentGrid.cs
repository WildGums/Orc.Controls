namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class AlignmentGrid : FrameworkElement<AlignmentGridModel>
    {
        public AlignmentGrid(AutomationElement element) 
            : base(element)
        {
        }
    }
}
