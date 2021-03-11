namespace Orc.Controls.Example.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using Catel;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Views;

    //public class WizardPageViewModelBase<TWizardPage> : ViewModelBase, IWizardPageViewModel
    //    where TWizardPage : class, IWizardPage
    public class WizardPageViewModelBase : ViewModelBase
    {
        #region Constructors
        //public WizardPageViewModelBase(TWizardPage wizardPage)
        //{
        //    Argument.IsNotNull(() => wizardPage);

        //    DeferValidationUntilFirstSaveCall = true;
        //    WizardPage = wizardPage;
        //    QuickNavigateToPage = new TaskCommand<IWizardPage>(QuickNavigateToPageExecuteAsync, QuickNavigateToPageCanExecute);
        //    ContentLoaded = new TaskCommand<StepBarItem>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
        //}
        public WizardPageViewModelBase()
        {
            DeferValidationUntilFirstSaveCall = true;
            ContentLoaded = new TaskCommand<StepBarItem>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
        }
        #endregion

        #region Properties

        //[Model(SupportIEditableObject = false)]
        //public TWizardPage WizardPage { get; private set; }

        //public IWizard Wizard
        //{
        //    get
        //    {
        //        var wizardPage = WizardPage;
        //        if (wizardPage is null)
        //        {
        //            return null;
        //        }

        //        return wizardPage.Wizard;
        //    }
        //}

        public virtual void EnableValidationExposure()
        {
            DeferValidationUntilFirstSaveCall = false;

            Validate(true);
        }
        #endregion

        #region Commands

        //public TaskCommand<IWizardPage> QuickNavigateToPage { get; private set; }

        //public bool QuickNavigateToPageCanExecute(IWizardPage parameter)
        //{
        //    if (!Wizard.AllowQuickNavigation)
        //    {
        //        return false;
        //    }

        //    if (!parameter.IsVisited)
        //    {
        //        return false;
        //    }

        //    if (Wizard.CurrentPage == parameter)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public async Task QuickNavigateToPageExecuteAsync(IWizardPage parameter)
        //{
        //    var page = parameter;
        //    if (page != null && page.IsVisited && Wizard.Pages is System.Collections.Generic.List<IWizardPage>)
        //    {
        //        var list = Wizard.Pages.ToList();
        //        var index = list.IndexOf(page);

        //        await Wizard.MoveToPageAsync(index);
        //    }
        //}

        public TaskCommand<StepBarItem> ContentLoaded { get; private set; }

        public bool ContentLoadedCanExecute(StepBarItem stepBarItem)
            => true;

        public async Task ContentLoadedExecuteAsync(StepBarItem stepBarItem)
        {
            Grid grid = (Grid)stepBarItem.GetType().GetField("grid").GetValue(this);
            TextBlock txtTitle = (TextBlock)stepBarItem.GetType().GetField("txtTitle").GetValue(this);
            Rectangle pathline = (Rectangle)stepBarItem.GetType().GetField("pathline").GetValue(this);
            Ellipse ellipse = (Ellipse)stepBarItem.GetType().GetField("ellipse").GetValue(this);

            if (stepBarItem.Orientation == Orientation.Horizontal)
            {
                //stepBarItem.grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 100.0);
                grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 100.0);
                grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 8 });
                txtTitle.SetCurrentValue(Grid.ColumnProperty, 0);
                txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                pathline.SetCurrentValue(Grid.ColumnProperty, 1);
                pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
                pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                pathline.SetCurrentValue(FrameworkElement.WidthProperty, 48.0);
                pathline.SetCurrentValue(FrameworkElement.HeightProperty, 2.0);
                pathline.SetCurrentValue(Canvas.TopProperty, 13.0);
                ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 240.0);
                grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 240.0);
                grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 56 });
                txtTitle.SetCurrentValue(Grid.ColumnProperty, 1);
                txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                pathline.SetCurrentValue(Grid.ColumnProperty, 0);
                pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(2));
                pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                pathline.SetCurrentValue(FrameworkElement.WidthProperty, 2.0);
                pathline.SetCurrentValue(FrameworkElement.HeightProperty, 48.0);
                pathline.SetCurrentValue(Canvas.TopProperty, 35.0);
                ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
            }
        }
        #endregion
    }
}
