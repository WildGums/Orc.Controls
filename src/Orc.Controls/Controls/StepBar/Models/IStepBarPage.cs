namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public interface IStepBarPage
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

        Task CancelAsync();
        Task SaveAsync();

        /// <summary>
        /// Executes once all the pages of the wizard have been saved.
        /// </summary>
        /// <returns></returns>
        Task AfterWizardPagesSavedAsync();
    }
}
