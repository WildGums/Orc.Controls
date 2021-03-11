namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Security.RightsManagement;
    using System.Threading.Tasks;
    using Catel.MVVM;

    public interface IWizardPage
    {
        ISummaryItem GetSummary();

        IWizard Wizard { get; set; }

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
