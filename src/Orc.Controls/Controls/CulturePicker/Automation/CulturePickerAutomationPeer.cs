namespace Orc.Controls.Controls.Automation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using Orc.Controls.Automation;

    public class CulturePickerAutomationPeer : CommandAutomationPeerBase
    {
        private readonly CulturePicker _culturePicker;

        private readonly ComboBoxAutomationPeer _comboBoxAutomationPeer;

        public CulturePickerAutomationPeer(CulturePicker culturePicker)
            : base(culturePicker)
        {
            _culturePicker = culturePicker;

            _comboBoxAutomationPeer = new ComboBoxAutomationPeer(_culturePicker.CultureCombobox);
        }

        protected override string GetClassNameCore()
        {
            return typeof(CulturePicker).FullName;
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

        [CommandRunMethod]
        public List<string> GetAvailableCultures()
        {
            return (_culturePicker.ViewModel as CulturePickerViewModel)?.AvailableCultures.Select(x => x.Name).ToList() ?? new List<string>();
        }

        //#region Value Pattern
        //string IValueProvider.Value => _culturePicker.SelectedCulture?.EnglishName;
        //bool IValueProvider.IsReadOnly => _culturePicker.CultureCombobox.IsReadOnly;

        //void IValueProvider.SetValue(string value)
        //{
        //    var culture = CultureInfo.GetCultures(CultureTypes.AllCultures)
        //        .FirstOrDefault(x => x.EnglishName == value);

        //    if (culture is null)
        //    {
        //        return;
        //    }

        //    _culturePicker.SetCurrentValue(CulturePicker.SelectedCultureProperty, culture);
        //}
        //#endregion
    }
}
