namespace Orc.Controls
{
    public interface IStepBarItem
    {
        string Title { get; set; }

        int Number { get; set; }

        StepBarItemStates State { get; set; }
    }
}
