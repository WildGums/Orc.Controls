namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.StepBarItem))]
    public class StepBarItem : FrameworkElement<StepBarItemModel, StepBarItemMap>
    {
        public StepBarItem(AutomationElement element)
            : base(element)
        {
        }

        public string Number => Map.EllipseTextBlock?.Value ?? string.Empty;
        public string Title => Map.TitleTextBlock?.Value ?? string.Empty;

        public void Invoke()
        {
            Map.ExecuteButton?.Click();
        }
    }
}
