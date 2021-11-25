namespace Orc.Automation
{
    using System;

    public class AutomationException : Exception
    {
        public AutomationException()
        {
        }

        public AutomationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
