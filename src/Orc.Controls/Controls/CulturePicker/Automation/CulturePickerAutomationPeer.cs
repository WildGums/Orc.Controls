namespace Orc.Controls.Controls.Automation
{
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;

    public class CulturePickerAutomationPeer : FrameworkElementAutomationPeer, IValueProvider
    {
        private readonly CulturePicker _culturePicker;

        private readonly ComboBoxAutomationPeer _comboBoxAutomationPeer;

        public CulturePickerAutomationPeer(CulturePicker culturePicker)
            : base(culturePicker)
        {
            _culturePicker = culturePicker;

            _comboBoxAutomationPeer = new ComboBoxAutomationPeer(_culturePicker.CultureCombobox);
        }

        public override object GetPattern(PatternInterface pattern)
        {
            if (pattern == PatternInterface.Value)
            {
                return this;
            }

            if (pattern == PatternInterface.SelectionItem)
            {
                return _comboBoxAutomationPeer;
            }    
            
            if (pattern == PatternInterface.ExpandCollapse)
            {
                return _comboBoxAutomationPeer;
            }

            if (pattern == PatternInterface.Scroll)
            {
                return _comboBoxAutomationPeer;
            }

            return base.GetPattern(pattern);
        }

        #region Value Pattern
        string IValueProvider.Value => _culturePicker.SelectedCulture?.EnglishName;
        bool IValueProvider.IsReadOnly => _culturePicker.CultureCombobox.IsReadOnly;

        void IValueProvider.SetValue(string value)
        {
            var culture = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .FirstOrDefault(x => x.EnglishName == value);

            if (culture is null)
            {
                return;
            }

            _culturePicker.SetCurrentValue(CulturePicker.SelectedCultureProperty, culture);
        }
        #endregion
    }
}
