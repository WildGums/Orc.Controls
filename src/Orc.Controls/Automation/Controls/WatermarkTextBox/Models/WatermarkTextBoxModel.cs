namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [ActiveAutomationModel]
    public class WatermarkTextBoxModel : FrameworkElementModel
    {
        public WatermarkTextBoxModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Text { get; set; }
        public string Watermark { get; set; }
        public bool SelectAllOnGotFocus { get; set; }
        //public DataTemplate WatermarkTemplate { get; set; }
    }
}
