namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class DropDownButtonView : AutomationBase
    {
        private readonly DropDownButtonMap _map;

        public DropDownButtonView(AutomationElement element) 
            : base(element)
        {
            _map = new DropDownButtonMap(element);
        }

        public TResult InvokeInDropDownScope<TResult>(Func<Menu, TResult> action)
        {
            Menu menu = null;
            using (new DisposableToken<DropDownButtonView>(this, _ => menu = OpenDropDown(), _ => CloseDropDown()))
            {
                return action.Invoke(menu);
            }
        }

        public bool IsToggled
        {
            get => (bool)Element.GetToggleState();
            set => Element.TrySetToggleState(value);
        }

        public void CloseDropDown()
        {
            Element.TrySetToggleState(false);
        }

        public Menu OpenDropDown()
        {
            var windowHost = Element.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
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
