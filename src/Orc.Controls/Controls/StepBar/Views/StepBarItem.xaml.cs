namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Catel.Collections;

    public sealed partial class StepBarItem
    {
        #region Constructors
        public StepBarItem()
        {
            Loaded += OnLoaded;

            InitializeComponent();
        }
        #endregion Constructors

        #region Properties
        public StepBarViewModel StepBarViewModel
        {
            get { return (StepBarViewModel)GetValue(StepBarViewModelProperty); }
            set { SetValue(StepBarViewModelProperty, value); }
        }

        public static readonly DependencyProperty StepBarViewModelProperty = DependencyProperty.Register(nameof(StepBarViewModel), typeof(StepBarViewModel),
            typeof(StepBarItem), new PropertyMetadata(null, (sender, e) => ((StepBarItem)sender).OnStepBarViewModelChanged()));

        public IStepBarItem Item
        {
            get { return (IStepBarItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(IStepBarItem),
            typeof(StepBarItem), new PropertyMetadata(null, (sender, e) => ((StepBarItem)sender).OnItemChanged()));


        public IStepBarItem SelectedItem
        {
            get { return (IStepBarItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(IStepBarItem),
            typeof(StepBarItem), new PropertyMetadata(null, (sender, e) => ((StepBarItem)sender).OnSelectedItemChanged()));


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

        public string IconUri
        {
            get { return (string)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        public static readonly DependencyProperty IconUriProperty = DependencyProperty.Register(nameof(IconUri), typeof(string),
            typeof(StepBarItem));
        #endregion Properties

        #region Methods
        private void OnStepBarViewModelChanged()
        {
            OnItemChanged();

            if (IconUri != null)
            {
                ellipse.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                icon.SetCurrentValue(Image.SourceProperty, new BitmapImage(
                    new Uri($"/Orc.Controls;component/{IconUri}")));
                icon.SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }

        private void OnItemChanged()
        {
            var item = Item;
            if (item != null)
            {
                SetCurrentValue(NumberProperty, item.Number);
                SetCurrentValue(TitleProperty, item.BreadcrumbTitle ?? item.Title);
                SetCurrentValue(DescriptionProperty, item.Description);

                if (StepBarViewModel != null)
                    pathline.SetCurrentValue(VisibilityProperty, StepBarViewModel.IsLastItem(item) ? Visibility.Collapsed : Visibility.Visible);
            }
        }

        private void OnSelectedItemChanged()
        {
            if (SelectedItem == null)
                return;

            var isSelected = ReferenceEquals(SelectedItem, Item);
            var isCompleted = Item.Number < SelectedItem.Number;
            var isVisited = Item.IsVisited;

            if (StepBarViewModel != null)
                SetCurrentValue(CursorProperty, (StepBarViewModel.AllowQuickNavigation && isVisited) ? System.Windows.Input.Cursors.Hand : null);
            UpdateContent(isCompleted);
            UpdateSelection(isSelected, isCompleted, isVisited);
        }

        private void UpdateSelection(bool isSelected, bool isCompleted, bool isVisited)
        {
            UpdateShapeColor(pathline, isCompleted && !isSelected);
            UpdateShapeColor(ellipse, isSelected || isVisited);

            txtTitle.SetCurrentValue(TextBlock.ForegroundProperty, isSelected ?
                TryFindResource("Orc.Brushes.Black") : (isVisited ?
                TryFindResource("Orc.Brushes.GrayBrush2") :
                TryFindResource("Orc.Brushes.GrayBrush1")));
        }

        private void UpdateContent(bool isCompleted)
        {
            ellipseText.SetCurrentValue(VisibilityProperty, isCompleted ? Visibility.Hidden : Visibility.Visible);
            ellipseCheck.SetCurrentValue(VisibilityProperty, isCompleted ? Visibility.Visible : Visibility.Hidden);
        }

        private SolidColorBrush GetAccentColorBrush(bool isSelected = true)
        {
            //Argument.IsNotNull(() => frameworkElement);

            var resourceName = isSelected ? ThemingKeys.AccentColorBrush : ThemingKeys.AccentColorBrush40;

            var brush = (SolidColorBrush)TryFindResource(resourceName);
            /*if (brush is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Theming is not yet initialized, make sure to initialize a theme via ThemeManager first");
            }*/

            return brush;
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
            var targetColor = GetAccentColorBrush(isSelected).Color;

            var colorAnimation = new ColorAnimation(fromColor, (Color)targetColor, WizardConfiguration.AnimationDuration);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Fill.(SolidColorBrush.Color)", ArrayShim.Empty<object>()));

            storyboard.Children.Add(colorAnimation);

            storyboard.Begin(shape);
        }
        #endregion Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
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
    }
}
