#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.StepBar))]
public class StepBar : FrameworkElement<StepBarModel, StepBarMap>
{
    public StepBar(AutomationElement element) 
        : base(element)
    {
            
    }

    public IReadOnlyList<StepBarItem> Items => By.Many<StepBarItem>();

    public StepBarItem SelectedItem
    {
        get => Map.ItemList.SelectedItem?.Find<StepBarItem>();
        //set => Map.ItemList.SelectedItem = value?.Find(controlType: ControlType.ListItem, scope: TreeScope.Parent);
    }
}
#nullable enable
