namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.Data;


    public static class AutomationElementBaseExtensions
    {

        public static object CreateControlMap(this AutomationElementBase element, Type mapType)
        {
            Argument.IsNotNull(() => element);

            return AutomationElementExtensions.CreateControlMap(element, mapType);
        }

        public static void MouseClick(this AutomationElementBase element, MouseButton mouseButton = MouseButton.Left)
        {
            element.Element.MouseClick(mouseButton);
        }

        public static TElement FindAncestor<TElement>(this AutomationElementBase child, Func<AutomationElement, bool> searchFunc)
            where TElement : AutomationElementBase
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            return child.Element.FindAncestor<TElement>(searchFunc);
        }

        public static AutomationElement FindAncestor(this AutomationElementBase child, Func<AutomationElement, bool> searchFunc)
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            return child.Element.FindAncestor(searchFunc);
        }

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
