namespace Orc.Controls.Controls.StepBar.Models
{
    /// <summary>
    /// An INavigationStrategy object defines how forward and backward
    /// navigation occurs within the wizard when the user clicks the "Next" or
    /// the "Back" button. Providing a custom implementation of this interface
    /// allows to customize the default behaviour of moving forward/backward
    /// one page at a time.
    /// </summary>
    public interface INavigationStrategy
    {
        /// <summary>
        /// Returns the index of the wizard page that should be displayed if
        /// the user clicks the "Next" button. Returns
        /// <em>Orc.Wizard.WizardConfiguration.CannotNavigate</em> to indicate
        /// that navigating forward is not possible.
        /// </summary>
        int GetIndexOfNextPage(IWizard wizard);

        /// <summary>
        /// Returns the index of the wizard page that should be displayed if
        /// the user clicks the "Back" button. Returns
        /// <em>Orc.Wizard.WizardConfiguration.CannotNavigate</em> to indicate
        /// that navigating backward is not possible.
        /// </summary>
        int GetIndexOfPreviousPage(IWizard wizard);
    }
}
