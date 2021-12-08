namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Catel;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Tab))]
    public class Tab : FrameworkElement
    {
        public Tab(AutomationElement element)
            : base(element, ControlType.Tab)
        {
        }

        public IReadOnlyList<TabItem> Items => By.ControlType(ControlType.TabItem).Many<TabItem>();

        public int SelectedIndex
        {
            get => Array.FindIndex(Items.ToArray(), x => x.IsSelected);
            set => SelectedItem = this[value];
        }

        public string SelectedItemHeader
        {
            get => SelectedItem?.Header;
            set => SelectedItem = this[value];
        }

        public TabItem SelectedItem
        {
            get => Items.FirstOrDefault(x => x.IsSelected);
            set => value?.TrySelect();
        }

        public TabItem this[string header]
        {
            get => Items.FirstOrDefault(x => Equals(x.Header, header));
        }

        public TabItem this[int index]
        {
            get
            {
                if (index >= Items.Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(index)} should be less than '{Items?.Count ?? 0}' and  greater than '{0}'");
                }

                return Items[index];
            }
        }

        public IDisposable TabScope(int tabIndex, bool shouldReturnToPreviousTab = false)
        {
            var previouslySelectedIndex = SelectedIndex;
            if (Equals(previouslySelectedIndex, tabIndex))
            {
                return new DisposableToken(this, _ => { }, _ => { });
            }

            return new DisposableToken<Tab>(this,
                    x =>
                    {
                        x.Instance.SelectedIndex = tabIndex;
                        Wait.UntilResponsive();
                    },
                    x =>
                    {
                        if (shouldReturnToPreviousTab)
                        {
                            x.Instance.SelectedIndex = previouslySelectedIndex;
                        }
                    });
        }
    }
}
