namespace Orc.Controls
{
    using Catel.Data;

    public class StepBarItemBase : ModelBase, IStepBarItem
    {
        public string Title { get; }
        public int Number { get; }
        public StepBarItemStates State { get; set; }
    }
}
