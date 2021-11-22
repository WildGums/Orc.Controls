namespace Orc.Automation
{
    public interface IResultToAutomationCommandResultConverter
    {
        AutomationMethodResult Convert(object result);
    }

    public interface IResultToAutomationCommandResultConverter<in T> : IResultToAutomationCommandResultConverter
    {
        AutomationMethodResult Convert(T result);
    }
}
