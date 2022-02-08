namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using Orc.Automation;
    using FrameworkElement = Orc.Automation.Controls.FrameworkElement;

    [AutomatedControl(Class = typeof(Controls.DropDownButton))]
    public class DropDownButton : FrameworkElement
    {
        private DropDownButtonView _view;

        public DropDownButton(AutomationElement element)
            : base(element)
        {
        }

        public DropDownButtonView View => _view ??= new DropDownButtonView(Element);

        public bool IsChecked
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public object Header
        {
            get => Access.GetValue<object>();
            set => Access.SetValue(value);
        }

        public DropdownArrowLocation ArrowLocation
        {
            get => Access.GetValue<DropdownArrowLocation>();
            set => Access.SetValue(value);
        }

        public Thickness ArrowMargin
        {
            get => Access.GetValue<Thickness>();
            set => Access.SetValue(value);
        }

        public bool IsArrowVisible
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public void AddMenuItems(List<string> items)
        {
            Access.Execute(nameof(DropDownButtonAutomationPeer.AddMenuItems), items);
        }
    }
}
