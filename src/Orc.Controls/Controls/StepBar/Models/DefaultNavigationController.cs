namespace Orc.Controls.Controls.StepBar.Models
{
    using System.Collections.Generic;
    using System.Net;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class DefaultNavigationController : INavigationController
    {
        protected readonly ILanguageService _languageService;
        protected readonly IMessageService _messageService;

        private readonly List<IWizardNavigationButton> _wizardNavigationButtons = new List<IWizardNavigationButton>();

        public DefaultNavigationController(IWizard wizard, ILanguageService languageService, IMessageService messageService)
        {
            Argument.IsNotNull(() => wizard);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => languageService);

            Wizard = wizard;
            _languageService = languageService;
            _messageService = messageService;
        }

        public IWizard Wizard { get; }

        public IEnumerable<IWizardNavigationButton> GetNavigationButtons()
        {
            if (_wizardNavigationButtons.Count == 0)
            {
                _wizardNavigationButtons.AddRange(CreateNavigationButtons(Wizard));
            }

            return _wizardNavigationButtons;
        }

        public void EvaluateNavigationCommands()
        {
            _wizardNavigationButtons.ForEach(x =>
            {
                x.Update();
            });
        }

        protected virtual IEnumerable<IWizardNavigationButton> CreateNavigationButtons(IWizard wizard)
        {
            var buttons = new List<WizardNavigationButton>();

            buttons.Add(CreateBackButton(wizard));
            buttons.Add(CreateForwardButton(wizard));
            buttons.Add(CreateFinishButton(wizard));
            buttons.Add(CreateCancelButton(wizard));

            return buttons;
        }

        protected virtual WizardNavigationButton CreateBackButton(IWizard wizard)
        {
            var button = new WizardNavigationButton
            {
                Content = _languageService.GetString("Wizard_Back"),
                IsVisibleEvaluator = () => !wizard.IsFirstPage(),
                Command = new TaskCommand(async () =>
                {
                    await wizard.MoveBackAsync();
                },
                () =>
                {
                    if (!wizard.HandleNavigationStates)
                    {
                        return true;
                    }

                    return wizard.CanMoveBack;
                })
            };

            return button;
        }

        protected virtual WizardNavigationButton CreateForwardButton(IWizard wizard)
        {
            var button = new WizardNavigationButton
            {
                Content = _languageService.GetString("Wizard_Next"),
                IsVisibleEvaluator = () => !wizard.IsLastPage(),
                Command = new TaskCommand(async () =>
                {
                    await wizard.MoveForwardAsync();
                },
                () =>
                {
                    if (!wizard.HandleNavigationStates)
                    {
                        return true;
                    }

                    return wizard.CanMoveForward;
                })
            };

            return button;
        }

        protected virtual WizardNavigationButton CreateFinishButton(IWizard wizard)
        {
            var button = new WizardNavigationButton
            {
                Content = _languageService.GetString("Wizard_Finish"),
                IsVisibleEvaluator = () => wizard.IsLastPage(),
                Command = new TaskCommand(async () =>
                {
                    await wizard.ResumeAsync();
                },
                () =>
                {
                    if (!wizard.HandleNavigationStates)
                    {
                        return true;
                    }

                    if (!Wizard.CanResume)
                    {
                        return false;
                    }

                    // Don't validate
                    var validationSummary = wizard.GetValidationContextForCurrentPage(false);
                    if (!validationSummary.HasErrors)
                    {
                        return true;
                    }

                    return false;
                })
            };

            return button;
        }

        protected virtual WizardNavigationButton CreateCancelButton(IWizard wizard)
        {
            var button = new WizardNavigationButton
            {
                Content = _languageService.GetString("Wizard_Cancel"),
                IsVisible = true,
                Command = new TaskCommand(async () =>
                {
                    if (await _messageService.ShowAsync(_languageService.GetString("Wizard_AreYouSureYouWantToCancelWizard"), button: MessageButton.YesNo) == MessageResult.No)
                    {
                        return;
                    }

                    await Wizard.CancelAsync();
                },
                () =>
                {
                    if (!wizard.HandleNavigationStates)
                    {
                        return true;
                    }

                    return wizard.CanCancel;
                })
            };

            return button;
        }
    }
}
