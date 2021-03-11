namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Threading;
    using Orc.Controls.Controls.StepBar.Models;

    public static class IWizardExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static async Task MoveForwardOrResumeAsync(this IWizard wizard)
        {
            Argument.IsNotNull(() => wizard);

            if (wizard.CanMoveForward)
            {
                Log.Debug("Moving forward from MoveNextOrResumeAsync()");

                await wizard.MoveForwardAsync();
                return;
            }

            if (wizard.CanResume)
            {
                Log.Debug("Resuming from MoveNextOrResumeAsync()");

                await wizard.ResumeAsync();
                return;
            }

            Log.Debug("Could not move forward or resume from MoveNextOrResumeAsync()");
        }

        public static Task MoveToPageAsync(this IWizard wizard, IWizardPage wizardPage)
        {
            var index = wizard.Pages.ToList().IndexOf(wizardPage);
            if (index < 0)
            {
                return TaskHelper.Completed;
            }

            return wizard.MoveToPageAsync(index);
        }

        public static IWizardPage AddPage(this IWizard wizard, IWizardPage page)
        {
            Argument.IsNotNull(() => wizard);
            Argument.IsNotNull(() => page);

            wizard.InsertPage(wizard.Pages.Count(), page);

            return page;
        }

        public static TWizardPage AddPage<TWizardPage>(this IWizard wizard)
            where TWizardPage : IWizardPage
        {
            Argument.IsNotNull(() => wizard);

            return wizard.InsertPage<TWizardPage>(wizard.Pages.Count());
        }

        public static TWizardPage InsertPage<TWizardPage>(this IWizard wizard, int index)
            where TWizardPage : IWizardPage
        {
            Argument.IsNotNull(() => wizard);

            var typeFactory = wizard.GetTypeFactory();
            var page = typeFactory.CreateInstance<TWizardPage>();

            wizard.InsertPage(index, page);

            return page;
        }

        public static TWizardPage AddPage<TWizardPage>(this IWizard wizard, object model)
            where TWizardPage : IWizardPage
        {
            Argument.IsNotNull(() => wizard);

            return wizard.InsertPage<TWizardPage>(wizard.Pages.Count(), model);
        }

        public static TWizardPage InsertPage<TWizardPage>(this IWizard wizard, int index, object model)
            where TWizardPage : IWizardPage
        {
            Argument.IsNotNull(() => wizard);

            var typeFactory = wizard.GetTypeFactory();
            var page = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TWizardPage>(model);

            wizard.InsertPage(index, page);

            return page;
        }

        public static TWizardPage FindPageByType<TWizardPage>(this IWizard wizard)
            where TWizardPage : IWizardPage
        {
            return (TWizardPage)FindPage(wizard, x => typeof(TWizardPage).IsAssignableFromEx(x.GetType()));
        }

        public static IWizardPage FindPage(this IWizard wizard, Func<IWizardPage, bool> predicate)
        {
            Argument.IsNotNull(() => wizard);
            Argument.IsNotNull(() => predicate);

            var allPages = wizard.Pages.ToList();
            if (allPages.Count == 0)
            {
                return null;
            }

            return allPages.FirstOrDefault(predicate);
        }

        public static bool IsFirstPage(this IWizard wizard, IWizardPage wizardPage = null)
        {
            return IsPage(wizard, wizardPage, x => x.First());
        }

        public static bool IsLastPage(this IWizard wizard, IWizardPage wizardPage = null)
        {
            return IsPage(wizard, wizardPage, x => x.Last());
        }

        private static bool IsPage(this IWizard wizard, IWizardPage wizardPage, Func<List<IWizardPage>, IWizardPage> selector)
        {
            Argument.IsNotNull(() => wizard);

            if (wizardPage is null)
            {
                wizardPage = wizard.CurrentPage;
            }

            var allPages = wizard.Pages.ToList();
            if (allPages.Count == 0)
            {
                return false;
            }

            var isLastPage = ReferenceEquals(selector(allPages), wizardPage);
            return isLastPage;
        }
    }
}
