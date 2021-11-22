namespace Orc.Automation
{
    using System.Globalization;

    public class CultureToAutomationResultConverter : ResultToAutomationCommandResultConverter<CultureInfo>
    {
        public override AutomationMethodResult Convert(CultureInfo culture)
        {
            return new AutomationMethodResult
            {
                Data = AutomationValue.FromValue(culture.Name, typeof(string))
            };
        }
    }
}
