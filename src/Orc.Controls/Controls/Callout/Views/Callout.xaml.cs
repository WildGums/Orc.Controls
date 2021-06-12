
namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;
    using Catel.MVVM.Views;

    [ContentProperty(nameof(InnerContent))]
    public partial class Callout
    {
        static Callout()
        {
            typeof(Callout).AutoDetectViewPropertiesToSubscribe();
        }

        public Callout()
        {
            InitializeComponent();

            Popup.Opened += PopupOnOpened;
        }

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object InnerContent
        {
            get { return GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register(nameof(InnerContent), typeof(object), typeof(Callout));


        [ViewToViewModel(viewModelPropertyName: nameof(CalloutViewModel.Name), MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string CalloutName
        {
            get { return (string)GetValue(CalloutNameProperty); }
            set { SetValue(CalloutNameProperty, value); }
        }

        public static readonly DependencyProperty CalloutNameProperty = DependencyProperty.Register(nameof(CalloutName),
            typeof(string), typeof(Callout), new PropertyMetadata(null));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public object PlacementTarget
        {
            get { return GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }

        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.Register(nameof(PlacementTarget), typeof(object), typeof(Callout),
                new PropertyMetadata((sender, args) => ((Callout)sender).OnPlacementTargetChanged(args)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(Callout));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(nameof(IsClosable),
            typeof(bool), typeof(Callout), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan ShowTime
        {
            get { return (TimeSpan)GetValue(ShowTimeProperty); }
            set { SetValue(ShowTimeProperty, value); }
        }

        public static readonly DependencyProperty ShowTimeProperty = DependencyProperty.Register(nameof(ShowTime),
            typeof(TimeSpan), typeof(Callout), new PropertyMetadata(TimeSpan.FromSeconds(30)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command),
            typeof(ICommand), typeof(Callout), new PropertyMetadata(null));


        public PlacementMode Placement
        {
            get { return (PlacementMode)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
            nameof(Placement), typeof(PlacementMode), typeof(Callout), new PropertyMetadata(PlacementMode.Bottom,
                (sender, args) => ((Callout)sender).OnPlacementChanged(args)));


        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset), typeof(double), typeof(Callout), new PropertyMetadata(default(double),
                (sender, args) => ((Callout)sender).OnVerticalOffsetChanged(args)));


        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
            nameof(HorizontalOffset), typeof(double), typeof(Callout), new PropertyMetadata(default(double),
                (sender, args) => ((Callout)sender).OnHorizontalOffsetChanged(args)));

        public double TailBaseWidth
        {
            get { return (double)GetValue(TailBaseWidthProperty); }
            set { SetValue(TailBaseWidthProperty, value); }
        }

        public static readonly DependencyProperty TailBaseWidthProperty = DependencyProperty.Register(
            nameof(TailBaseWidth), typeof(double), typeof(Callout), new PropertyMetadata(20d,
                (sender, args) => ((Callout)sender).OnTailBaseWidthChanged(args)));

        public HorizontalAlignment TailHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TailHorizontalAlignmentProperty); }
            set { SetValue(TailHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TailHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(TailHorizontalAlignment), typeof(HorizontalAlignment), typeof(Callout), new PropertyMetadata(default(HorizontalAlignment),
                (sender, args) => ((Callout)sender).OnTailHorizontalAlignmentChanged(args)));

        public VerticalAlignment TailVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(TailVerticalAlignmentProperty); }
            set { SetValue(TailVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TailVerticalAlignmentProperty = DependencyProperty.Register(
            nameof(TailVerticalAlignment), typeof(VerticalAlignment), typeof(Callout), new PropertyMetadata(default(VerticalAlignment),
                (sender, args) => ((Callout)sender).OnTailVerticalAlignmentChanged(args)));

        private void OnTailVerticalAlignmentChanged(DependencyPropertyChangedEventArgs args)
        {

        }


        private void OnTailHorizontalAlignmentChanged(DependencyPropertyChangedEventArgs args)
        {
            //UpdatePopupPosition();
        }


        private void OnTailBaseWidthChanged(DependencyPropertyChangedEventArgs args)
        {
            //UpdatePopupPosition();
        }

        private void OnHorizontalOffsetChanged(DependencyPropertyChangedEventArgs args)
        {
            //  UpdatePopupPosition();
        }

        private void OnVerticalOffsetChanged(DependencyPropertyChangedEventArgs args)
        {
            // UpdatePopupPosition();
        }

        private void OnPlacementTargetChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is FrameworkElement oldPlacementTarget)
            {
                oldPlacementTarget.SizeChanged -= OnPlacementTargetSizeChanged;
            }

            if (args.NewValue is FrameworkElement newPlacementTarget)
            {
                newPlacementTarget.SizeChanged += OnPlacementTargetSizeChanged;
            }
        }

        private void OnPlacementChanged(DependencyPropertyChangedEventArgs args)
        {
            //UpdatePopupPosition();
        }

        private CustomPopupPlacement[] OnCustomPopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            const double DropShadowSize = 2d;

            var placementTarget = PlacementTarget as FrameworkElement;
            if (placementTarget is null)
            {
                return Array.Empty<CustomPopupPlacement>();
            }

            var popupHalfWidth = popupSize.Width / 2d;
            var popupHalfHeight = popupSize.Height / 2d;

            var targetSizeHalfWidth = targetSize.Width / 2d;
            var targetSizeHalfHeight = targetSize.Height / 2d;

            var x = targetSizeHalfWidth - popupHalfWidth;
            var y = 0d - (targetSizeHalfHeight + popupHalfHeight);

            var dropShadowOffset = DropShadowSize;

            switch (Placement)
            {
                case PlacementMode.Top:
                    y -= popupHalfHeight;
                    y -= dropShadowOffset;
                    break;

                case PlacementMode.Bottom:
                    y = 0;
                    break;

                case PlacementMode.Left:
                    x -= targetSizeHalfWidth;
                    x -= popupHalfWidth;
                    break;

                case PlacementMode.Right:
                    x += targetSizeHalfWidth;
                    x += popupHalfWidth;
                    break;

                default:
                    // NOTE:Vladimir: Will throw if not supported
                    throw new NotSupportedException($"Callout placement = '{Placement}' not supported. Supported modes: '{PlacementMode.Left}', '{PlacementMode.Top}', '{PlacementMode.Right}', '{PlacementMode.Bottom}'");
            }

            // Offset is handled by managing Tail size
            // y += verticalOffset;
            // x += horizontalOffset;

            UpdatePopupPosition();

            return new CustomPopupPlacement[]
            {
                new CustomPopupPlacement
                {
                    Point = new Point(x, y),
                    PrimaryAxis = PopupPrimaryAxis.None
                }
            };
        }


        private void PopupOnOpened(object sender, EventArgs e)
        {
            UpdatePopupPosition();
        }

        private void OnPlacementTargetSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePopupPosition();
        }

        protected override void OnLoaded(EventArgs e)
        {
            //UpdatePopupPosition();
        }

        private void UpdatePopupPosition()
        {
            var placementTarget = PlacementTarget;
            if (placementTarget is null)
            {
                return;
            }

            UpdateTail(Placement);
        }

        private void UpdateTail(PlacementMode placementMode)
        {
            // Refresh tail appearance
            var styleName = GetTailPolygonStyleResourceName();
            var style = TryFindResource(styleName);
            TailPolygon.SetValue(FrameworkElement.StyleProperty, style);

            if (placementMode == PlacementMode.Left || placementMode == PlacementMode.Right)
            {
                TailPolygon.SetCurrentValue(FrameworkElement.WidthProperty, HorizontalOffset);
            }

            if (placementMode == PlacementMode.Top || placementMode == PlacementMode.Bottom)
            {
                TailPolygon.SetCurrentValue(FrameworkElement.HeightProperty, VerticalOffset);
            }

            switch (placementMode)
            {
                case PlacementMode.Top:
                    BorderGapRectangle.SetCurrentValue(HorizontalAlignmentProperty, TailHorizontalAlignment);
                    BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    BorderGapRectangle.SetCurrentValue(WidthProperty, TailBaseWidth - 3);
                    BorderGapRectangle.SetCurrentValue(HeightProperty, 2d);

                    // Position having in mind margin from TailPolygon
                    var margin = TailPolygon.Margin;
                    BorderGapRectangle.SetCurrentValue(MarginProperty, new Thickness(margin.Left + 1, margin.Top + 1, 0, 0));
                    break;
            }
        }

        private string GetTailPolygonStyleResourceName()
        {
            var tailAlignment = Placement == PlacementMode.Left || Placement == PlacementMode.Right ? (object)TailVerticalAlignment : TailHorizontalAlignment;

            return $"{Placement}{tailAlignment}PolygonStyle";
        }
    }
}
