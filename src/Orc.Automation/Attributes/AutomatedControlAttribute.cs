namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;

    public class AutomatedControlAttribute : AutomationAttribute
    {
        private string _className;

        public string ClassName
        {
            get => _className ?? Class?.FullName;
            set => _className = value;
        }

        public ControlType ControlType => AutomationHelper.GetControlType(ControlTypeName);

        public Type Class { get; set; }
        public string ControlTypeName { get; set; }
    }
}
