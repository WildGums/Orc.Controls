namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using Catel.Data;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class ValidationContextTreeModel : FrameworkElementModel
    {
        public ValidationContextTreeModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public IValidationContext ValidationContext { get; set; }
        public bool ShowErrors { get; set; }
        public bool ShowWarnings { get; set; }
        public string Filter { get; set; }
        public IEnumerable<IValidationContextTreeNode> Nodes { get; set; }
        public bool IsExpandedByDefault { get; set; }
    }
}
