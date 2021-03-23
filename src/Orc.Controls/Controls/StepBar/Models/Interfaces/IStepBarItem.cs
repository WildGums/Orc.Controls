namespace Orc.Controls
{
    public interface IStepBarItem
    {
        string Title { get; }

        int Number { get; }

        StepBarItemStates State { get; set; }
    }
}
