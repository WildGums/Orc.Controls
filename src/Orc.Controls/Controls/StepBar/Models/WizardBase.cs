namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Threading;

    public abstract class WizardBase : ModelBase, IWizard
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IList<IWizardPage> _pages = new List<IWizardPage>();
        protected readonly ITypeFactory _typeFactory;

        private int _currentIndex = 0;
        private IWizardPage _currentPage;

        private INavigationStrategy _navigationStrategy = new DefaultNavigationStrategy();
        private INavigationController _navigationController;
        #endregion

        // Note: we can't remove this constructor, it would be a breaking change
        protected WizardBase(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;
            _navigationController = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<DefaultNavigationController>(this);

            ResizeMode = System.Windows.ResizeMode.NoResize;
            MinSize = new System.Windows.Size(650d, 500d);
            MaxSize = new System.Windows.Size(650d, 500d);

            ShowInTaskbar = false;
            IsHelpVisible = false;
            CanShowHelp = true;
            HandleNavigationStates = true;
            AllowQuickNavigation = false;
        }

        #region Properties
        public IWizardPage CurrentPage
        {
            get
            {
                if (_currentPage is null)
                {
                    // Try to set page (probably first page)
                    SetCurrentPage(_currentIndex);
                }

                return _currentPage;
            }
        }

        public IEnumerable<IWizardPage> Pages
        {
            get { return _pages.AsEnumerable(); }
        }

        public INavigationStrategy NavigationStrategy
        {
            get { return _navigationStrategy; }
            protected set { _navigationStrategy = value; }
        }

        public INavigationController NavigationController
        {
            get { return _navigationController; }
            protected set { _navigationController = value; }
        }

        public string Title { get; protected set; }

        public virtual System.Windows.ResizeMode ResizeMode { get; protected set; }

        public virtual System.Windows.Size MinSize { get; protected set; }

        public virtual System.Windows.Size MaxSize { get; protected set; }

        public virtual bool HandleNavigationStates { get; protected set; }

        public virtual bool CanResume
        {
            get
            {
                var remainingPages = Pages.Skip(_currentIndex + 1).ToList();
                if (remainingPages.Count == 0)
                {
                    return true;
                }

                // Make sure we can move next at all
                if (!CanMoveForward)
                {
                    return false;
                }

                if (remainingPages.All(x => (x is SummaryWizardPage == true) || x.IsOptional))
                {
                    return true;
                }

                return false;
            }
        }

        public virtual bool CanCancel
        {
            get { return true; }
        }

        public virtual bool CanMoveForward
        {
            get
            {
                var validationContext = GetValidationContextForCurrentPage(true);
                if (validationContext.HasErrors)
                {
                    return false;
                }

                var indexOfNextPage = NavigationStrategy.GetIndexOfNextPage(this);
                return (indexOfNextPage != WizardConfiguration.CannotNavigate);
            }
        }

        public virtual bool CanMoveBack
        {
            get
            {
                int indexOfPreviousPage = NavigationStrategy.GetIndexOfPreviousPage(this);
                return (indexOfPreviousPage != WizardConfiguration.CannotNavigate);
            }
        }

        public bool IsHelpVisible { get; protected set; }

        public bool CanShowHelp { get; protected set; }

        public bool ShowInTaskbar { get; protected set; }

        public bool AllowQuickNavigation { get; protected set; }
        #endregion

        #region Events
        public event EventHandler<EventArgs> CurrentPageChanged;
        public event EventHandler<EventArgs> MovedForward;
        public event EventHandler<EventArgs> MovedBack;
        public event EventHandler<EventArgs> Canceled;
        public event EventHandler<EventArgs> Resumed;
        public event EventHandler<EventArgs> HelpShown;
        #endregion

        #region Methods
        public void InsertPage(int index, IWizardPage page)
        {
            Argument.IsNotNull(() => page);

            Log.Debug("Adding page '{0}' to index '{1}'", page.GetType().GetSafeFullName(false), index);

            page.Wizard = this;

            _pages.Insert(index, page);

            UpdatePageNumbers();
        }

        public void RemovePage(IWizardPage page)
        {
            Argument.IsNotNull(() => page);

            for (var i = 0; i < _pages.Count; i++)
            {
                if (ReferenceEquals(page, _pages[i]))
                {
                    Log.Debug("Removing page '{0}' at index '{1}'", page.GetType().GetSafeFullName(false), i);

                    page.Wizard = null;
                    _pages.RemoveAt(i--);
                }
            }

            UpdatePageNumbers();
        }

        public virtual IValidationContext GetValidationContextForCurrentPage(bool validate = true)
        {
            if (_currentPage != null)
            {
                var vm = _currentPage.ViewModel;
                if (vm != null)
                {
                    if (validate)
                    {
                        vm.Validate(true);
                    }

                    return vm.ValidationContext;
                }
            }

            return new ValidationContext();
        }

        public virtual async Task MoveForwardAsync()
        {
            if (!CanMoveForward)
            {
                if (_currentPage?.ViewModel is IWizardPageViewModel wizardPageViewModel)
                {
                    wizardPageViewModel.EnableValidationExposure();
                }

                return;
            }

            var currentPage = _currentPage;
            if (currentPage != null)
            {
                var vm = currentPage.ViewModel;
                if (vm != null)
                {
                    var result = await vm.SaveAndCloseViewModelAsync();
                    if (!result)
                    {
                        return;
                    }
                }
            }

            var indexOfNextPage = NavigationStrategy.GetIndexOfNextPage(this);
            SetCurrentPage(indexOfNextPage);

            RaiseMovedForward();
        }

        public virtual async Task MoveBackAsync()
        {
            if (!CanMoveBack)
            {
                return;
            }

            var indexOfPreviousPage = NavigationStrategy.GetIndexOfPreviousPage(this);
            SetCurrentPage(indexOfPreviousPage);

            RaiseMovedBack();
        }

        public virtual async Task MoveToPageAsync(int indexOfNextPage)
        {
            // Note: we skip the navigation strategy when going directly to another page

            // Note: for now make a *big* assumption that a lower index is backward navigation
            var isForward = indexOfNextPage > _currentIndex;
            if (isForward)
            {
                if (!await ValidateAndSaveCurrentPageAsync())
                {
                    return;
                }
            }

            SetCurrentPage(indexOfNextPage);
        }

        protected virtual async Task<bool> ValidateAndSaveCurrentPageAsync()
        {
            var validationContext = GetValidationContextForCurrentPage(true);
            if (validationContext.HasErrors)
            {
                if (_currentPage?.ViewModel is IWizardPageViewModel wizardPageViewModel)
                {
                    wizardPageViewModel.EnableValidationExposure();
                }

                return false;
            }

            var currentPage = _currentPage;
            if (currentPage != null)
            {
                var vm = currentPage.ViewModel;
                if (vm != null)
                {
                    var result = await vm.SaveAndCloseViewModelAsync();
                    if (!result)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual Task InitializeAsync()
        {
            return TaskHelper.Completed;
        }

        /*[ObsoleteEx(ReplacementTypeOrMember = "ResumeAsync", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public virtual Task SaveAsync()
        {
            return ResumeAsync();
        }*/

        public virtual async Task ResumeAsync()
        {
            if (!CanResume)
            {
                return;
            }

            Log.Debug("Saving wizard '{0}'", GetType().GetSafeFullName(false));

            // ORCOMP-590: Fix for final view model
            if (!await ValidateAndSaveCurrentPageAsync())
            {
                return;
            }

            foreach (var page in _pages)
            {
                await page.SaveAsync();
            }

            foreach (var page in _pages)
            {
                await page.AfterWizardPagesSavedAsync();
            }

            RaiseResumed();
        }

        public virtual async Task CancelAsync()
        {
            if (!CanCancel)
            {
                return;
            }

            Log.Debug("Canceling wizard '{0}'", GetType().GetSafeFullName(false));

            foreach (var page in _pages)
            {
                await page.CancelAsync();
            }

            RaiseCanceled();
        }

        public virtual Task CloseAsync()
        {
            return TaskHelper.Completed;
        }

        public virtual async Task ShowHelpAsync()
        {
            if (!CanShowHelp)
            {
                return;
            }

            HelpShown?.Invoke(this, EventArgs.Empty);
        }

        protected internal virtual IWizardPage SetCurrentPage(int newIndex)
        {
            Log.Debug("Setting current page index to '{0}'", newIndex);

            var currentPage = _currentPage;
            if (currentPage != null)
            {
                currentPage.ViewModelChanged -= OnPageViewModelChanged;

                var vm = currentPage.ViewModel;
                if (vm != null)
                {
                    vm.PropertyChanged -= OnPageViewModelPropertyChanged;
                }
            }

            var newPage = _pages[newIndex];
            if (newPage != null)
            {
                newPage.ViewModelChanged += OnPageViewModelChanged;

                var vm = newPage.ViewModel;
                if (vm != null)
                {
                    vm.PropertyChanged += OnPageViewModelPropertyChanged;
                }
            }

            _currentPage = newPage;
            _currentIndex = newIndex;

            RaisePropertyChanged(nameof(CurrentPage));
            CurrentPageChanged?.Invoke(this, EventArgs.Empty);

            NavigationController.EvaluateNavigationCommands();

            if (newPage is not null)
            {
                newPage.IsVisited = true;
            }

            return newPage;
        }

        private void OnPageViewModelChanged(object sender, ViewModelChangedEventArgs e)
        {
            var oldVm = e.OldViewModel;
            if (oldVm != null)
            {
                oldVm.PropertyChanged -= OnPageViewModelPropertyChanged;
            }

            var newVm = e.NewViewModel;
            if (newVm != null)
            {
                newVm.PropertyChanged += OnPageViewModelPropertyChanged;
            }
        }

        private void OnPageViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(CanMoveBack));
            RaisePropertyChanged(nameof(CanMoveForward));
            RaisePropertyChanged(nameof(CanResume));
            RaisePropertyChanged(nameof(CanCancel));

            NavigationController.EvaluateNavigationCommands();
        }

        private void UpdatePageNumbers()
        {
            var counter = 1;

            foreach (var page in _pages)
            {
                page.Number = counter++;
            }
        }

        protected void RaiseResumed()
        {
            Resumed?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseCanceled()
        {
            Canceled?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseMovedBack()
        {
            MovedBack?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseMovedForward()
        {
            MovedForward?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
