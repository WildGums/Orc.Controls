namespace Orc.Controls.Controls.StepBar.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Catel.Collections;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Controls.Controls.StepBar.Models;
    using Orc.Controls.Controls.StepBar.ViewModels;

    public sealed partial class StepBarItem
    {
        public StepBarViewModel StepBarViewModel
        {
            get { return (StepBarViewModel)GetValue(StepBarViewModelProperty); }
            set { SetValue(StepBarViewModelProperty, value); }
        }

        public static readonly DependencyProperty StepBarViewModelProperty = DependencyProperty.Register(nameof(StepBarViewModel), typeof(StepBarViewModel),
            typeof(StepBarItem));

        public StepBarItem()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            OnPageChanged();
        }

        public IStepBarPage Page
        {
            get { return (IStepBarPage)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        public static readonly DependencyProperty PageProperty = DependencyProperty.Register(nameof(Page), typeof(IStepBarPage),
            typeof(StepBarItem), new PropertyMetadata(null, (sender, e) => ((StepBarItem)sender).OnPageChanged()));


        public IStepBarPage CurrentPage
        {
            get { return (IStepBarPage)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(IStepBarPage),
            typeof(StepBarItem), new PropertyMetadata(null, (sender, e) => ((StepBarItem)sender).OnCurrentPageChanged()));


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
            typeof(StepBarItem), new PropertyMetadata(string.Empty));


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string),
            typeof(StepBarItem), new PropertyMetadata(string.Empty));


        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(nameof(Number), typeof(int),
            typeof(StepBarItem), new PropertyMetadata(0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBarItem), new PropertyMetadata(Orientation.Vertical));

        private void OnPageChanged()
        {
            var page = Page;
            if (page != null)
            {
                SetCurrentValue(NumberProperty, page.Number);
                SetCurrentValue(TitleProperty, page.BreadcrumbTitle ?? page.Title);
                SetCurrentValue(DescriptionProperty, page.Description);

                if (StepBarViewModel != null)
                    pathline.SetCurrentValue(VisibilityProperty, StepBarViewModel.IsLastPage(Page) ? Visibility.Collapsed : Visibility.Visible);
            }
        }

        private void OnCurrentPageChanged()
        {
            var isSelected = ReferenceEquals(CurrentPage, Page);
            var isCompleted = Page.Number < CurrentPage.Number;
            var isVisited = Page.IsVisited;

            if (StepBarViewModel != null)
                SetCurrentValue(CursorProperty, (StepBarViewModel.AllowQuickNavigation && isVisited) ? System.Windows.Input.Cursors.Hand : null);
            UpdateContent(isCompleted);
            UpdateSelection(isSelected, isCompleted, isVisited);
        }

        private void UpdateSelection(bool isSelected, bool isCompleted, bool isVisited)
        {
            UpdateShapeColor(pathline, isCompleted && !isSelected);
            UpdateShapeColor(ellipse, isSelected || isVisited);

            txtTitle.SetCurrentValue(System.Windows.Controls.TextBlock.ForegroundProperty, isSelected ?
                TryFindResource("Orc.Brushes.Black") : (isVisited ?
                TryFindResource("Orc.Brushes.GrayBrush2") :
                TryFindResource("Orc.Brushes.GrayBrush1")));
        }

        private void UpdateContent(bool isCompleted)
        {
            ellipseText.SetCurrentValue(VisibilityProperty, isCompleted ? Visibility.Hidden : Visibility.Visible);
            ellipseCheck.SetCurrentValue(VisibilityProperty, isCompleted ? Visibility.Visible : Visibility.Hidden);
        }

        private void UpdateShapeColor(Shape shape, bool isSelected)
        {
            var storyboard = new Storyboard();

            if (shape != null && shape.Fill is null)
            {
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
                shape.Fill = (SolidColorBrush)TryFindResource(ThemingKeys.AccentColorBrush40);
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.
            }

            var fromColor = ((SolidColorBrush)shape?.Fill)?.Color ?? Colors.Transparent;
            var targetColor = this.GetAccentColorBrush(isSelected).Color;

            var colorAnimation = new ColorAnimation(fromColor, (Color)targetColor, WizardConfiguration.AnimationDuration);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Fill.(SolidColorBrush.Color)", ArrayShim.Empty<object>()));

            storyboard.Children.Add(colorAnimation);

            storyboard.Begin(shape);
        }

        private void setOrientation()
        {
            if (Orientation == Orientation.Horizontal)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setOrientation();
        }
    }
}
