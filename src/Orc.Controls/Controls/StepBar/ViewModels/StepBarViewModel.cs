namespace Orc.Controls.Controls.StepBar.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;
    using Orc.Controls.Controls.StepBar.Views;

    public class StepBarViewModel : ViewModelBase
    {
        private int _currentIndex = 0;
        public IStepBarPage CurrentPage { get; set; }

        public IList<IStepBarPage> Pages { get; set; } = new List<IStepBarPage>();

        public bool AllowQuickNavigation { get; set; } = true;

        public StepBarViewModel()
        {
            Pages.Add(new AgeWizardPage());
            Pages.Add(new AgeWizardPage());
            Pages.Add(new AgeWizardPage());
            Pages.Add(new AgeWizardPage());
            //Pages.Add(new SummaryWizardPage());

            CurrentPage = Pages.First();

            QuickNavigateToPage = new TaskCommand<IStepBarPage>(QuickNavigateToPageExecuteAsync, QuickNavigateToPageCanExecute);
            ContentLoaded = new TaskCommand<StepBarItem>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
        }

        public virtual async Task MoveForwardAsync()
        {
            if (_currentIndex < Pages.Count - 1)
            {
                Pages[_currentIndex].IsVisited = true;
                _currentIndex++;
                CurrentPage = Pages[_currentIndex];
                Pages[_currentIndex].IsVisited = true;
            }
        }

        public virtual async Task MoveBackAsync()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                CurrentPage = Pages[_currentIndex];
            }
        }

        protected internal async Task SetCurrentPage(int newIndex)
        {
            if (_currentIndex >= 0 && _currentIndex < Pages.Count)
            {
                _currentIndex = newIndex;
                CurrentPage = Pages[_currentIndex];
            }
        }

        public bool IsLastPage(IStepBarPage page)
        {
            return page.Equals(Pages.LastOrDefault());
        }

        public TaskCommand<IStepBarPage> QuickNavigateToPage { get; private set; }

        public bool QuickNavigateToPageCanExecute(IStepBarPage parameter)
        {
            /*if (!Wizard.AllowQuickNavigation)
            {
                return false;
            }

            if (!parameter.IsVisited)
            {
                return false;
            }

            if (Wizard.CurrentPage == parameter)
            {
                return false;
            }*/

            return true;
        }

        public async Task QuickNavigateToPageExecuteAsync(IStepBarPage parameter)
        {
            var page = parameter;
            if (page != null && page.IsVisited && Pages is List<IStepBarPage>)
            {
                var index = Pages.IndexOf(page);

                await SetCurrentPage(index);
            }
        }

        public TaskCommand<StepBarItem> ContentLoaded { get; private set; }

        public bool ContentLoadedCanExecute(StepBarItem stepBarItem)
            => true;

        public async Task ContentLoadedExecuteAsync(StepBarItem stepBarItem)
        {
            if (stepBarItem.Orientation == Orientation.Horizontal)
            {
                stepBarItem.grid.ColumnDefinitions.Add(new ColumnDefinition());
                stepBarItem.grid.RowDefinitions.Add(new RowDefinition());
                stepBarItem.grid.RowDefinitions.Add(new RowDefinition());
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 100.0);
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 100.0);
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 8 });
                stepBarItem.txtTitle.SetCurrentValue(Grid.ColumnProperty, 0);
                stepBarItem.txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                stepBarItem.pathline.SetCurrentValue(Grid.ColumnProperty, 1);
                stepBarItem.pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
                stepBarItem.pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                stepBarItem.pathline.SetCurrentValue(FrameworkElement.WidthProperty, 48.0);
                stepBarItem.pathline.SetCurrentValue(FrameworkElement.HeightProperty, 2.0);
                stepBarItem.pathline.SetCurrentValue(Canvas.TopProperty, 13.0);
                stepBarItem.ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            }
            else
            {
                stepBarItem.grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                stepBarItem.grid.ColumnDefinitions.Add(new ColumnDefinition());
                stepBarItem.grid.RowDefinitions.Add(new RowDefinition());
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 240.0);
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 240.0);
                stepBarItem.grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 56 });
                stepBarItem.txtTitle.SetCurrentValue(Grid.ColumnProperty, 1);
                stepBarItem.txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                stepBarItem.pathline.SetCurrentValue(Grid.ColumnProperty, 0);
                stepBarItem.pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(2));
                stepBarItem.pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                stepBarItem.pathline.SetCurrentValue(FrameworkElement.WidthProperty, 2.0);
                stepBarItem.pathline.SetCurrentValue(FrameworkElement.HeightProperty, 48.0);
                stepBarItem.pathline.SetCurrentValue(Canvas.TopProperty, 35.0);
                stepBarItem.ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
            }
        }
    }
}
