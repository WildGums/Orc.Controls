namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;

    public static class AutomationControlExtensions
    {
        public static void MouseClick(this AutomationControl control, MouseButton mouseButton = MouseButton.Left)
        {
            control.Element.MouseClick(mouseButton);
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
