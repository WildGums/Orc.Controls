#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class LogMessageCategoryToggleButton : FrameworkElement<LogMessageCategoryToggleButtonModel, LogMessageCategoryToggleButtonMap>
{
    public LogMessageCategoryToggleButton(AutomationElement element)
        : base(element)
    {
    }

    public bool IsToggled
    {
        get => Map.Toggle.IsToggled == true;
        set => Map.Toggle.IsToggled = value;
    }

    public int EntryCount
    {
        get
        {
            var value = Map.EntryCountText.Value;
            return int.TryParse(value, out var entryCount) ? entryCount : 0;
        }
    }
    public string Category => Map.CategoryText.Value;
}
#nullable enable
