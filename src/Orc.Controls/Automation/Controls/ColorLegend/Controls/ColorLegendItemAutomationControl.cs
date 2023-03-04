#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.ListItem))]
public class ColorLegendItemAutomationControl : ListItem
{
    public ColorLegendItemAutomationControl(AutomationElement element)
        : base(element)
    {
    }

    private CheckBox CheckBox => By.One<CheckBox>();
    private ColorPicker Button => By.One<ColorPicker>();
    private Text AdditionalDataTextBlock => By.Id("AdditionalDataTextBlock").One<Text>();
    private Text DescriptionTextBlock => By.Id("DescriptionTextBlock").One<Text>();

    public string AdditionalText => AdditionalDataTextBlock.Value;
    public string Description => DescriptionTextBlock.Value;

    public bool IsChecked
    {
        //It only could be only 2 states
        get => (bool) CheckBox.IsChecked;
        set => CheckBox.IsChecked = value;
    }
}
#nullable enable
