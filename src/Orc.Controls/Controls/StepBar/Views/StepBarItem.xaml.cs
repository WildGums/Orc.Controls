namespace Orc.Controls.Controls.StepBar.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using Catel.Collections;
    using Orc.Controls.Controls.StepBar.Models;
    using Orc.Controls.Controls.StepBar.ViewModels;

    public sealed partial class StepBarItem
    {
        #region Constructors
        public StepBarItem()
        {
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
        #endregion Properties

        #region Methods
        private void OnStepBarViewModelChanged()
        {
            OnItemChanged();
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
        #endregion Methods
    }
}
