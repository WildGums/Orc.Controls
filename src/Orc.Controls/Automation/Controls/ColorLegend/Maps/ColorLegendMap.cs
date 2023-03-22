#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class ColorLegendMap : AutomationBase
{
    public ColorLegendMap(AutomationElement element) 
        : base(element)
    {
    }

    public Edit SearchBoxPart => By.One<Edit>();
    public Text FilterWatermarkTextPart => SearchBoxPart.By.One<Text>();
    public DropDownButton SettingsButtonPart => By.Id("SettingsButton").One<DropDownButton>();
    public CheckBox AllVisibleCheckBoxPart => By.Id("AllVisibleCheckBox").One<CheckBox>();
    public Text SelectedItemCountLabel => By.Id("SelectedItemCountLabel").One<Text>();
    public Button UnselectAllButtonPart => By.Id("UnselectAllButton").One<Button>();
    public List ListPart => By.One<List>();
    public List<ColorLegendItemAutomationControl> Items => By.Many<ColorLegendItemAutomationControl>();
}
#nullable enable
