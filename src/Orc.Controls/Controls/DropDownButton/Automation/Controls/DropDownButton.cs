namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.DropDownButton), ControlTypeName = nameof(ControlType.Button))]
    public class DropDownButton : FrameworkElement
    {
        public DropDownButton(AutomationElement element)
            : base(element, ControlType.Button)
        {
        }

        public TResult InvokeInDropDownScope<TResult>(Func<Menu, TResult> action)
        {
            Menu menu = null;
            using (new DisposableToken<DropDownButton>(this, _ => menu = OpenDropDown(), _ => CloseDropDown()))
            {
                return action.Invoke(menu);
            }
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
