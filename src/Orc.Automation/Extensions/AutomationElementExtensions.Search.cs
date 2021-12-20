namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using Automation;
    using Catel;
    using Catel.Linq;

    public static partial class AutomationElementExtensions
    {
        public static TAutomationElementOrElementCollection FindOneOrMany<TAutomationElementOrElementCollection>(this AutomationElement element, SearchContext searchContext)
        {
            Argument.IsNotNull(() => element);

            object searchResult;
            var type = typeof(TAutomationElementOrElementCollection);
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                searchResult = element.FindAll(id: searchContext.Id, name: searchContext.Name, className: searchContext.ClassName, controlType: searchContext.ControlType);
            }
            else
            {
                searchResult = //!searchInfo.IsTransient && _targetControl is not null
                                //  ? _targetControl.GetPart(id: searchInfo.AutomationId, name: searchInfo.Name, className: searchInfo.ClassName, controlType: searchInfo.ControlType)
                                // :
                                element.Find(id: searchContext.Id, name: searchContext.Name, className: searchContext.ClassName, controlType: searchContext.ControlType);
            }

            searchResult = (TAutomationElementOrElementCollection)AutomationHelper.WrapAutomationObject(type, searchResult);

            return (TAutomationElementOrElementCollection)searchResult;
        }


        public static TElement Find<TElement>(this AutomationElement element, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            return Find<TElement>(element, new SearchContext(id, name, className, controlType), scope, numberOfWaits);
        }

        public static AutomationElement Find(this AutomationElement element, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            return Find(element, new SearchContext(id, name, className, controlType), scope, numberOfWaits);
        }

        public static TElement Find<TElement>(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            return (TElement)Find(element, searchContext, typeof(TElement), scope, numberOfWaits);
        }

        public static object Find(this AutomationElement element, SearchContext searchContext, Type wrapperType, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var className = AutomationHelper.GetControlClassName(wrapperType);
            if (!string.IsNullOrWhiteSpace(className) && string.IsNullOrWhiteSpace(searchContext.ClassName))
            {
                searchContext.ClassName = className;
            }

            var controlType = AutomationHelper.GetControlType(wrapperType);
            if (controlType is not null && searchContext.ControlType is null)
            {
                searchContext.ControlType = controlType;
            }

            var foundElement = Find(element, searchContext, scope, numberOfWaits);
            if (foundElement is null)
            {
                return null;
            }

            return AutomationHelper.WrapAutomationObject(wrapperType, foundElement);
        }

        public static AutomationElement Find(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var id = searchContext.Id;
            var name = searchContext.Name;
            var controlType = searchContext.ControlType;
            var className = searchContext.ClassName;

            var conditions = new List<Condition>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
            }

            if (!string.IsNullOrWhiteSpace(className))
            {
                conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
            }

            if (controlType is not null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
            }

            if (!conditions.Any())
            {
                return null;
            }

            var resultCondition = conditions.Count == 1
                ? conditions[0]
                : new AndCondition(conditions.ToArray());

            return Find(element, resultCondition, scope, numberOfWaits);
        }

        public static AutomationElement Find(this AutomationElement element, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            Argument.IsNotNull(() => element);

            var numWaits = 0;

            AutomationElement foundElement;
            do
            {
                foundElement = TryFindElementByCondition(element, scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } 
            while (foundElement is null && numWaits < numberOfWaits);

            return foundElement;
        }

        private static AutomationElement TryFindElementByCondition(this AutomationElement element, TreeScope scope, Condition condition)
        {
            if (scope is TreeScope.Parent or TreeScope.Ancestors)
            {
                return element.GetParent(condition);
            }

            return element.FindFirst(scope, condition);
        }

        public static IEnumerable<TElement> FindAll<TElement>(this AutomationElement element, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            return FindAll<TElement>(element, new SearchContext(id, name, className, controlType), scope, numberOfWaits);
        }

        public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            return FindAll(element, new SearchContext(id, name, className, controlType), scope, numberOfWaits);
        }

        public static IEnumerable<TElement> FindAll<TElement>(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            return (IEnumerable<TElement>)FindAll(element, searchContext, typeof(TElement), scope, numberOfWaits);
        }

        public static object FindAll(this AutomationElement element, SearchContext searchContext, Type wrapperType, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var foundElement = FindAll(element, searchContext, scope, numberOfWaits);
            if (foundElement is null)
            {
                return null;
            }

            return AutomationHelper.WrapAutomationObject(wrapperType, foundElement);
        }

        public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var id = searchContext.Id;
            var name = searchContext.Name;
            var controlType = searchContext.ControlType;
            var className = searchContext.ClassName;

            var conditions = new List<Condition>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
            }

            if (!string.IsNullOrWhiteSpace(className))
            {
                conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
            }

            if (controlType is not null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
            }

            if (!conditions.Any())
            {
                return null;
            }

            var resultCondition = conditions.Count == 1
                ? conditions[0]
                : new AndCondition(conditions.ToArray());

            return FindAll(element, resultCondition, scope, numberOfWaits);
        }

        public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            Argument.IsNotNull(() => element);

            var numWaits = 0;

            IEnumerable<AutomationElement> foundElements;
            do
            {
                foundElements = element.TryFindElementsByCondition(scope, condition);

                ++numWaits;
                Thread.Sleep(100);
            } 
            while (foundElements is null && numWaits < numberOfWaits);

            return foundElements;
        }

        private static IEnumerable<AutomationElement> TryFindElementsByCondition(this AutomationElement element, TreeScope scope, Condition condition)
        {
            if (scope == TreeScope.Parent)
            {
                var parent = element.GetParent(condition);
                return parent is not null ? new List<AutomationElement> { parent } : null;
            }

            if (scope == TreeScope.Ancestors)
            {
                return element.GetAncestors(condition).ToList();
            }

            return element.FindAll(scope, condition)?.OfType<AutomationElement>();
        }
    }
}
