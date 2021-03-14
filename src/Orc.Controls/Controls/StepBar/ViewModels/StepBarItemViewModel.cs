namespace Orc.Controls.Controls.StepBar.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Views;

    public class StepBarItemViewModel : ViewModelBase
    {
        public StepBarItemViewModel()
        {
            ContentLoaded = new TaskCommand<StepBarItem>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
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
