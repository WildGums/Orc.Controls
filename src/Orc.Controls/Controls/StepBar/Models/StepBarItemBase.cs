namespace Orc.Controls
{
    using Catel.Data;

    public class StepBarItemBase : ModelBase, IStepBarItem
    {
        public string Title { get; set; }
        public int Number { get; set; }
        public StepBarItemStates State { get; set; }
    }
}
