namespace Orc.Controls
{
    using Catel.Data;

    public abstract class StepBarItemBase : ModelBase, IStepBarItem
    {
        public string Title { get; set; }
        public string BreadcrumbTitle { get; set; }
        public string Description { get; set; }
        public int Number { get; set; }
        public bool IsOptional { get; protected set; }
        public bool IsVisited { get; set; }
    }
}
