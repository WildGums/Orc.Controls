namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Globalization;
    using Orc.Controls;

    public class CulturePickerAutomationPeer : ControlRunMethodAutomationPeerBase<CulturePicker>
    {
        public CulturePickerAutomationPeer(CulturePicker culturePicker)
            : base(culturePicker)
        {
        }

        [AutomationMethod]
        public List<CultureInfo> GetAvailableCultures()
        {
            return (Control.ViewModel as CulturePickerViewModel)?.AvailableCultures ?? new List<CultureInfo>();
        }
    }
}
