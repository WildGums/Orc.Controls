namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Catel;

    public class ComboBox : FrameworkElement
    {
        public ComboBox(AutomationElement element) 
            : base(element, ControlType.ComboBox)
        {
        }

        public AutomationElement Select(int index)
        {
            if (Element.TrySelectItem(index, out var element))
            {
                return element;
            }

            return null;
        }

        public bool IsExpanded
        {
            get => Element.GetIsExpanded();
            set
            {
                var isExpanded = IsExpanded;
                if (Equals(isExpanded, value))
                {
                    return;
                }

                if (value)
                {
                    Element.Expand();
                }
                else
                {
                    Element.Collapse();
                }
            }
        }

        public TResult InvokeInExpandState<TResult>(Func<TResult> func)
        {
            using (new DisposableToken<ComboBox>(this, 
                x =>
                {
                    x.Instance.IsExpanded = true;
                    Wait.UntilResponsive();
                }, 
                x =>
                {
                    x.Instance.IsExpanded = false;
                    Wait.UntilResponsive();
                }))
            {
                return func.Invoke();
            }
        }

        public List<ListItem> Items
        {
            get
            {
                //To get items we need to expand combobox first, otherwise it won't work
                return InvokeInExpandState(() => By.ControlType(ControlType.ListItem).Many<ListItem>());
            }
        }
    }
}
