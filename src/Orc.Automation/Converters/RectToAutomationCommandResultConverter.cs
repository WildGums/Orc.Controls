namespace Orc.Automation
{
    using System.Windows;

    public class RectToAutomationCommandResultConverter : ResultToAutomationCommandResultConverter<Rect>
    {
        public override AutomationCommandResult Convert(Rect result)
        {
            return new AutomationCommandResult
            {
                Data = AutomationSendData.FromValue(new RectWrapper{Rect = result}, typeof(RectWrapper))
            };
        }
    }
}