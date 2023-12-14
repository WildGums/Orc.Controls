#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.CulturePicker))]
public class CulturePicker : FrameworkElement<CulturePickerModel, CulturePickerMap>
{
    public CulturePicker(AutomationElement element)
        : base(element)
    {
    }

    public string SelectedCulture
    {
        get => Map.Combobox.SelectedItem?.TryGetDisplayText();
        set => Map.Combobox.Select(x => Equals(x.TryGetDisplayText(), value));
    }

    public IReadOnlyList<string> Items
    {
        get
        {
            var combobox = Map.Combobox;
            return combobox.InvokeInExpandState(() => combobox.Items?.Select(x => x.TryGetDisplayText()).ToList());
        }
    }
}
#nullable enable
