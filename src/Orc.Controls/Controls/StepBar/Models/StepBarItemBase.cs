namespace Orc.Controls;

using System.Windows.Input;
using Catel.Data;

public class StepBarItemBase : ModelBase, IStepBarItem
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Number { get; set; }
    public StepBarItemStates State { get; set; }
    public ICommand? Command { get; set; }
}