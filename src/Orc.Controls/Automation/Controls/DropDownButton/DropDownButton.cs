namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.DropDownButton))]
    public class DropDownButton : FrameworkElement<DropDownButtonModel, DropDownButtonMap>
    {
        public DropDownButton(AutomationElement element)
            : base(element)
        {
            
        }

        public bool IsToggled
        {
            get => (bool)Element.GetToggleState();
            set => Element.TrySetToggleState(value);
        }

        public TResult InvokeInDropDownScope<TResult>(Func<Menu, TResult> action)
        {
            Menu menu = null;
            using (new DisposableToken(this, _ => menu = OpenDropDown(), _ => CloseDropDown()))
            {
                return action.Invoke(menu);
            }
        }

        public void InvokeInDropDownScope(Action<Menu> action)
        {
            Menu menu = null;
            using (new DisposableToken(this, _ => menu = OpenDropDown(), _ => CloseDropDown()))
            {
                action.Invoke(menu);
            }
        }

        public void CloseDropDown()
        {
            Element.TrySetToggleState(false);
        }

        public Menu OpenDropDown()
        {
            var windowHost = Element.GetHostWindow();
            if (windowHost is null)
            {
                return null;
            }

            Element.Toggle();

            Wait.UntilResponsive();

            var dropDownPopup = windowHost.Find(className: "Popup", controlType: ControlType.Window);
            var contextMenu = dropDownPopup?.Find(controlType: ControlType.Menu);

            return contextMenu?.As<Menu>();
        }
    }
}
