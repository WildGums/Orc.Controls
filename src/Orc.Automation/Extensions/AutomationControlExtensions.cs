namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;

    public static class AutomationControlExtensions
    {
        public static TScenario CreateScenario<TScenario>(this AutomationControl control)
        {
            Argument.IsNotNull(() => control);

            return control.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TScenario>(control);
        }

        public static TTemplate CreateControlMap<TTemplate>(this AutomationControl element)
        {
            Argument.IsNotNull(() => element);

            return (TTemplate)element.CreateControlMap(typeof(TTemplate));
        }

        public static object CreateControlMap(this AutomationControl control, Type mapType)
        {
            Argument.IsNotNull(() => control);

            return AutomationElementExtensions.CreateControlMap(control, mapType);
        }

        public static void MouseClick(this AutomationControl control, MouseButton mouseButton = MouseButton.Left)
        {
            control.Element.MouseClick(mouseButton);
        }

        public static TElement FindAncestor<TElement>(this AutomationControl child, Func<AutomationElement, bool> searchFunc)
            where TElement : AutomationControl
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            return child.Element.FindAncestor<TElement>(searchFunc);
        }

        public static AutomationElement FindAncestor(this AutomationControl child, Func<AutomationElement, bool> searchFunc)
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            return child.Element.FindAncestor(searchFunc);
        }

        public static AutomationElement Find(this AutomationControl parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            Argument.IsNotNull(() => parent);

            return parent.Element.Find(id, name, className, controlType, scope, numberOfWaits);
        }

        public static TElement Find<TElement>(this AutomationControl parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            Argument.IsNotNull(() => parent);

            return parent.Element.Find<TElement>(id, name, className, controlType, scope, numberOfWaits);
        }
    }
}
