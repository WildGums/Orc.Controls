namespace Orc.Controls.Automation
{
    using System.Windows.Media;
    using Orc.Automation;

    [AutomationAccessType]
    public class FontImageModel : FrameworkElementModel
    {
        public FontImageModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string ItemName { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public string FontFamily { get; set; }
    }
}
