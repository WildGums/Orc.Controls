namespace Orc.Automation
{
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using Orc.Controls;

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
        public CultureInfo GetAvailableCultures()
        {
            return (Control.ViewModel as CulturePickerViewModel)?.AvailableCultures.FirstOrDefault();
        }
    }
}
