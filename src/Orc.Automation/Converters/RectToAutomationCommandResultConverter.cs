namespace Orc.Automation
{
    using System.Windows;

    public class RectToAutomationCommandResultConverter : ResultToAutomationCommandResultConverter<Rect>
    {
        public override AutomationMethodResult Convert(Rect result)
        {
            return new AutomationMethodResult
            {
                Data = AutomationValue.FromValue(new RectWrapper{Rect = result}, typeof(RectWrapper))
            };
        }
    }
}