namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using Controls;

    public class CulturePickerAutomationPeer : ControlRunMethodAutomationPeerBase<CulturePicker>
    {
        private readonly ComboBoxAutomationPeer _comboBoxAutomationPeer;

        public CulturePickerAutomationPeer(CulturePicker culturePicker)
            : base(culturePicker)
        {
            _comboBoxAutomationPeer = new ComboBoxAutomationPeer(culturePicker.CultureCombobox);
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ComboBox;
        }

        public override object GetPattern(PatternInterface pattern)
        {
            if (pattern == PatternInterface.SelectionItem)
            {
                return _comboBoxAutomationPeer.GetPattern(pattern);
            }

            if (pattern == PatternInterface.ExpandCollapse)
            {
                return _comboBoxAutomationPeer.GetPattern(pattern);
            }

            if (pattern == PatternInterface.Scroll)
            {
                return _comboBoxAutomationPeer.GetPattern(pattern);
            }

            return base.GetPattern(pattern);
        }

        [AutomationMethod]
        public List<string> GetAvailableCultures()
        {
            return (Control.ViewModel as CulturePickerViewModel)?.AvailableCultures.Select(x => x.Name).ToList() ?? new List<string>();
        }
    }
}
