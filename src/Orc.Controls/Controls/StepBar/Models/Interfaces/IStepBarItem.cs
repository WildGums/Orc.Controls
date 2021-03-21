namespace Orc.Controls
{
    public interface IStepBarItem
    {
        string Title { get; set; }

        string BreadcrumbTitle { get; set; }

        string Description { get; set; }

        int Number { get; set; }

        bool IsOptional { get; }

        bool IsVisited { get; set; }
    }
}
