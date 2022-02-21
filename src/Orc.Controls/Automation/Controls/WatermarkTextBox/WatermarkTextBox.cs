namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.WatermarkTextBox), ControlTypeName = nameof(ControlType.Edit))]
    public class WatermarkTextBox : Edit
    {
        public WatermarkTextBox(AutomationElement element) 
            : base(element)
        {
        }

        public new WatermarkTextBoxModel Current => Model<WatermarkTextBoxModel>();
        public WatermarkTextBoxMap Map => Map<WatermarkTextBoxMap>();

        public string Watermark
        {
            get => Map.WatermarkText?.Value ?? string.Empty;
        }
    }
}
