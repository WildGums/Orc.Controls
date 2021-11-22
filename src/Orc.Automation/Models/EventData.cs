namespace Orc.Automation
{
    using System;

    public class AutomationEventArgs : EventArgs
    {
        public string EventName { get; set; }
        public object Data { get; set; }
    }
}
