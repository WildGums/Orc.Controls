namespace Orc.Automation
{
    using System.Globalization;

    public class CultureToAutomationResultConverter : ResultToAutomationCommandResultConverter<CultureInfo>
    {
        public override AutomationCommandResult Convert(CultureInfo culture)
        {
            return new AutomationCommandResult
            {
                Data = AutomationSendData.FromValue(culture.Name, typeof(string))
            };
        }
    }
}
