namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Window))]
    public class FindReplaceView : Window
    {
        public FindReplaceView(AutomationElement element) 
            : base(element)
        {
        }

        private FindReplaceViewMap Map => Map<FindReplaceViewMap>();
    }
}
