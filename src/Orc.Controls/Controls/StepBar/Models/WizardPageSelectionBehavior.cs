namespace Orc.Controls.Controls.StepBar.Models
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.MVVM.Views;
    using Catel.Windows.Interactivity;


    public class WizardPageSelectionBehavior : BehaviorBase<ContentControl>
    {
        #region Fields
        private IWizardPage _lastPage;
        #endregion

        #region Properties
        public IWizard Wizard
        {
            get { return (IWizard)GetValue(WizardProperty); }
            set { SetValue(WizardProperty, value); }
        }

        public static readonly DependencyProperty WizardProperty = DependencyProperty.Register(nameof(Wizard), typeof(IWizard),
            typeof(WizardPageSelectionBehavior), new PropertyMetadata(OnWizardChanged));
        #endregion

        private static void OnWizardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as WizardPageSelectionBehavior;
            if (behavior != null)
            {
                behavior.UpdatePage();

                var oldWizard = e.OldValue as IWizard;
                if (oldWizard != null)
                {
                    oldWizard.CurrentPageChanged -= behavior.OnCurrentPageChanged;
                    oldWizard.MovedBack -= behavior.OnMovedBack;
                    oldWizard.MovedForward -= behavior.OnMovedForward;
                }

                var wizard = e.NewValue as IWizard;
                if (wizard != null)
                {
                    wizard.CurrentPageChanged += behavior.OnCurrentPageChanged;
                    wizard.MovedBack += behavior.OnMovedBack;
                    wizard.MovedForward += behavior.OnMovedForward;
                }
            }
        }

        protected override void OnAssociatedObjectLoaded()
        {
            UpdatePage();
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            base.OnAssociatedObjectUnloaded();

            var wizard = Wizard;
            if (wizard is null)
            {
                return;
            }

            wizard.CurrentPageChanged -= OnCurrentPageChanged;
            wizard.MovedBack -= OnMovedBack;
            wizard.MovedForward -= OnMovedForward;

            SetCurrentValue(WizardProperty, null);
        }

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void OnMovedForward(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void OnMovedBack(object sender, EventArgs e)
        {
            UpdatePage();
        }

#pragma warning disable WPF0005 // Name of PropertyChangedCallback should match registered name.
        private void UpdatePage()
#pragma warning restore WPF0005 // Name of PropertyChangedCallback should match registered name.
        {
            if (AssociatedObject is null)
            {
                return;
            }

            var wizard = Wizard;
            if (wizard is null)
            {
                return;
            }

            if (_lastPage != null)
            {
                if (ReferenceEquals(_lastPage, wizard.CurrentPage))
                {
                    // Nothing has really changed
                    return;
                }

                _lastPage.ViewModel = null;
                _lastPage = null;
            }

            _lastPage = wizard.CurrentPage;

            var serviceLocator = this.GetServiceLocator();
            var viewModelLocator = serviceLocator.ResolveType<IWizardPageViewModelLocator>();
            var pageViewModelType = viewModelLocator.ResolveViewModel(_lastPage.GetType());

            var viewLocator = serviceLocator.ResolveType<IViewLocator>();
            var viewType = viewLocator.ResolveView(pageViewModelType);

            var typeFactory = serviceLocator.ResolveType<ITypeFactory>();
            var view = typeFactory.CreateInstance(viewType) as IView;
            if (view is null)
            {
                return;
            }

            var viewModelFactory = serviceLocator.ResolveType<IViewModelFactory>();
            var viewModel = viewModelFactory.CreateViewModel(pageViewModelType, wizard.CurrentPage, null);

            _lastPage.ViewModel = viewModel;

            view.DataContext = viewModel;
            AssociatedObject.SetCurrentValue(ContentControl.ContentProperty, view);
        }
    }
}
