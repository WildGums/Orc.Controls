namespace Orc.Controls.Controls.StepBar.Models
{
    using System.Collections.Generic;

    public interface INavigationController
    {
        IEnumerable<IWizardNavigationButton> GetNavigationButtons();
        void EvaluateNavigationCommands();
    }
}
