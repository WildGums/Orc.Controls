namespace Orc.Controls
{
    using System.Windows.Input;

    public interface IStepBarItem
    {
        string Title { get; }

        int Number { get; }

        StepBarItemStates State { get; set; }

        ICommand Command { get; }
    }
}
