namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using Catel.MVVM;

    public interface IStepBarItem
    {
        ISummaryItem GetSummary();

        IViewModel ViewModel { get; set; }

        event EventHandler<ViewModelChangedEventArgs> ViewModelChanged;

        string Title { get; set; }

        string BreadcrumbTitle { get; set; }

        string Description { get; set; }

        int Number { get; set; }

        bool IsOptional { get; }

        bool IsVisited { get; set; }
    }
}
