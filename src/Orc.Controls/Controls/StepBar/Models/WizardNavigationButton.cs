namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Windows.Input;
    using Catel.Data;
    using Catel.MVVM;

    public class WizardNavigationButton : ModelBase, IWizardNavigationButton
    {
        public WizardNavigationButton()
        {

        }

        public Func<string> ContentEvaluator { get; set; }

        public string Content { get; set; }

        public Func<bool> IsVisibleEvaluator { get; set; }

        public bool IsVisible { get; set; }

        public ICommand Command { get; set; }

        public void Update()
        {
            if (Command is ICatelCommand catelCommand)
            {
                catelCommand.RaiseCanExecuteChanged();
            }

            var contentEvaluator = ContentEvaluator;
            if (contentEvaluator is null == false)
            {
                Content = contentEvaluator();
            }

            var isVisibleEvaluator = IsVisibleEvaluator;
            if (isVisibleEvaluator is null == false)
            {
                IsVisible = isVisibleEvaluator();
            }
        }
    }
}
