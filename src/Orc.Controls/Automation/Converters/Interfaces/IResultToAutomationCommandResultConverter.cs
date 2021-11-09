namespace Orc.Controls.Automation
{
    public interface IResultToAutomationCommandResultConverter
    {
        AutomationCommandResult Convert(object result);
    }

    public interface IResultToAutomationCommandResultConverter<in T> : IResultToAutomationCommandResultConverter
    {
        AutomationCommandResult Convert(T result);
    }
}
