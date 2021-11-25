namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        public static AutomationElement GetParent(this AutomationElement childElement)
        {
            Argument.IsNotNull(() => childElement);

            return TreeWalker.ControlViewWalker.GetParent(childElement);
        }

        public static AutomationElement GetChild(this AutomationElement containerElement, int index)
        {
            Argument.IsNotNull(() => containerElement);

            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);
            var currentIndex = 0;

            while (!Equals(index, currentIndex) && item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);
                currentIndex++;
            }

            return item;
        }

        public static IEnumerable<AutomationElement> GetChildElements(this AutomationElement containerElement)
        {
            Argument.IsNotNull(() => containerElement);

            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);

            while (item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);

                yield return item;
            }
        }
    }
}
