namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Catel;

    [AutomatedControl(ControlTypeName = nameof(ControlType.ComboBox))]
    public class ComboBox : FrameworkElement
    {
        public ComboBox(AutomationElement element) 
            : base(element, ControlType.ComboBox)
        {
        }

        public int SelectedIndex
        {
            get
            {
                var selectedItem = Items.FirstOrDefault(x => x.GetIsSelected());
                if (selectedItem is null)
                {
                    return -1;
                }

                //TODO:It was quick solution, not all IReadOnlyLists should be ILists
                return ((IList<AutomationElement>)Items).IndexOf(selectedItem);
            }

            set
            {
                var items = Items;
                if (value >= items.Count || value < 0)
                {
                    return;
                }

                var item = items[value];
                InvokeInExpandState(() => item.Select());
            }
        }

        public AutomationElement SelectedItem
        {
            get => InvokeInExpandState(() => Element.GetSelection()?.FirstOrDefault());
            set => InvokeInExpandState(() => value?.Select());
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

        public void InvokeInExpandState(Action action)
        {
            InvokeInExpandState(() =>
            {
                action?.Invoke();
                return true;
            });
        }

        //TODO: Just return automation elements
        public IReadOnlyList<AutomationElement> Items
        {
            get
            {
                //To get items we need to expand combobox first, otherwise it won't work
                return InvokeInExpandState(() => (IReadOnlyList<AutomationElement>)By.ControlType(ControlType.ListItem).Many());
            }
        }

        public IReadOnlyList<T> GetItemsOfType<T>()
            where T : AutomationControl
        {
            return InvokeInExpandState(() => By.Many<T>());
        }
    }
}
