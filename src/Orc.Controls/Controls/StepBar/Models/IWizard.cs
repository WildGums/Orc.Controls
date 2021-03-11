namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Data;

    public interface IWizard
    {
        #region Properties
        IWizardPage CurrentPage { get; }
        IEnumerable<IWizardPage> Pages { get; }
        INavigationStrategy NavigationStrategy { get; }

        INavigationController NavigationController { get; }
        string Title { get; }
        System.Windows.ResizeMode ResizeMode { get; }
        System.Windows.Size MinSize { get; }
        System.Windows.Size MaxSize { get; }
        bool HandleNavigationStates { get; }
        bool CanResume { get; }
        bool CanCancel { get; }
        bool CanMoveForward { get; }
        bool CanMoveBack { get; }
        bool IsHelpVisible { get; }
        bool CanShowHelp { get; }
        bool ShowInTaskbar { get; }
        bool AllowQuickNavigation { get; }
        #endregion

        /*[ObsoleteEx(ReplacementTypeOrMember = "ResumeAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        Task SaveAsync();*/
        Task CancelAsync();
        Task ResumeAsync();

        Task InitializeAsync();
        Task CloseAsync();

        Task MoveForwardAsync();
        Task MoveBackAsync();
        Task MoveToPageAsync(int indexOfNextPage);
        Task ShowHelpAsync();

        event EventHandler<EventArgs> CurrentPageChanged;
        event EventHandler<EventArgs> MovedForward;
        event EventHandler<EventArgs> MovedBack;
        event EventHandler<EventArgs> Resumed;
        event EventHandler<EventArgs> Canceled;
        event EventHandler<EventArgs> HelpShown;

        void InsertPage(int index, IWizardPage page);
        void RemovePage(IWizardPage page);

        IValidationContext GetValidationContextForCurrentPage(bool validate = true);
    }
}
