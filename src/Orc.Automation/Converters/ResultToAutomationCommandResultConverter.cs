namespace Orc.Automation
{
    public abstract class ResultToAutomationCommandResultConverter<T> : IResultToAutomationCommandResultConverter<T>
    {
        public abstract AutomationMethodResult Convert(T result);

        AutomationMethodResult IResultToAutomationCommandResultConverter.Convert(object result)
        {
            return Convert((T)result);
        }
    }
}
