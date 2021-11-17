namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel;
    using Tests;

    public static class AutomationElementBaseExtensions
    {
        public static AutomationElement Find(this AutomationElementBase parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            Argument.IsNotNull(() => parent);

            return parent.Element.Find(id, name, className, controlType, scope, numberOfWaits);
        }

        public static TElement Find<TElement>(this AutomationElementBase parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationElementBase
        {
            Argument.IsNotNull(() => parent);

            return parent.Element.Find<TElement>(id, name, className, controlType, scope, numberOfWaits);
        }
    }
}
