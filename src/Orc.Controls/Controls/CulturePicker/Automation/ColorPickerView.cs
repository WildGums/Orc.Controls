namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class CulturePickerView : AutomationBase
    {
        private readonly CulturePickerMap _map;

        public CulturePickerView(AutomationElement element) 
            : base(element)
        {
            _map = new CulturePickerMap(element);
        }

        public string SelectedCulture
        {
            get => _map.Combobox.SelectedItem?.TryGetDisplayText();
            set => _map.Combobox.Select(x => Equals(x.TryGetDisplayText(), value));
        }

        public IReadOnlyList<string> Items
        {
            get
            {
                return _map.Combobox.InvokeInExpandState(() => _map.Combobox.Items?.Select(x => x.TryGetDisplayText()).ToList());
            }
        } 
    }
}
