namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        //public static AutomationElement GetAncestor(this AutomationElement element, Condition condition)
        //{
        //    Argument.IsNotNull(() => element);

        //    var treeWalker = new TreeWalker(condition);

        //    var isMatch = treeWalker.GetParent(element) is not null;
        //    var parent = GetParent(element);
        //    while (!isMatch && parent is not null)
        //    {
        //        isMatch = treeWalker.GetParent(parent) is not null;
        //        parent = GetParent(parent);
        //    }

        //    return parent;
        //}

        public static string TryGetDisplayText(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            var rawTreeWalker =TreeWalker.RawViewWalker;
            var rawElement = rawTreeWalker.GetFirstChild(element);

            return rawElement?.Current.Name;
        }

        public static IEnumerable<AutomationElement> GetAncestors(this AutomationElement element, Condition condition)
        {
            Argument.IsNotNull(() => element);

            var treeWalker = new TreeWalker(condition);

            var parent = GetParent(element);
            while (parent is not null)
            {
                if (treeWalker.GetParent(element) is not null)
                {
                    yield return parent;
                }

                parent = GetParent(parent);
            }
        }

        public static AutomationElement GetParent(this AutomationElement element, Condition condition = null)
        {
            Argument.IsNotNull(() => element);

            return condition is null ? TreeWalker.ControlViewWalker.GetParent(element) : new TreeWalker(condition).GetParent(element);
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

        public static AutomationElement GetFirstDescendant(this AutomationElement element, Condition condition)
        {
            Argument.IsNotNull(() => element);

            var treeWalker = new TreeWalker(condition);

            var item = treeWalker.GetFirstChild(element);

            return item;
        }

        public static IEnumerable<AutomationElement> GetChildElements(this AutomationElement containerElement)
        {
            Argument.IsNotNull(() => containerElement);

            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);

            while (item is not null)
            {
                yield return item;

                item = TreeWalker.ControlViewWalker.GetNextSibling(item);
            }
        }
    }
}
