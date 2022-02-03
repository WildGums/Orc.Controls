namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class CulturePicker : FrameworkElement
    {
        private CulturePickerView _view;

        public CulturePicker(AutomationElement element)
            : base(element)
        {
        }

        public CultureInfo SelectedCulture
        {
            get => (CultureInfo)Access.GetValue();
            set => Access.SetValue(value);
        }

        public List<CultureInfo> AvailableCultures
            =>  (List<CultureInfo>)Access.Execute(nameof(CulturePickerAutomationPeer.GetAvailableCultures));

        public CulturePickerView View => _view ??= new CulturePickerView(Element);
    }
}
