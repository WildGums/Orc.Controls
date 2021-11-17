namespace Orc.Automation
{
    public abstract class ResultToAutomationCommandResultConverter<T> : IResultToAutomationCommandResultConverter<T>
    {
        public abstract AutomationCommandResult Convert(T result);

        AutomationCommandResult IResultToAutomationCommandResultConverter.Convert(object result)
        {
            return Convert((T)result);
        }
    }
}