namespace Orc.Automation.Automation
{
    using System.Windows.Automation;

    public class SearchContext
    {
        public SearchContext()
        {
            
        }

        public SearchContext(string id = null, string name = null, string className = null, ControlType controlType = null)
        {
            Id = id;
            Name = name;
            ClassName = className;
            ControlType = controlType;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ControlType ControlType { get; set; }
        public string ClassName { get; set; }
        public bool IsCached { get; set; }

        public bool IsEmpty => string.IsNullOrWhiteSpace(Id)
                               && string.IsNullOrWhiteSpace(Name)
                               && string.IsNullOrWhiteSpace(ClassName)
                               && ControlType is null;
    }
}
