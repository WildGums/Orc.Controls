
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

        private void PopupOnOpened(object sender, EventArgs e)
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

            RefreshPolygonStyle();
            RefreshTailSizes();
        }

        private void RefreshPolygonStyle()
        {
            var styleName = GetTailPolygonStyleResourceName();
            var style = TryFindResource(styleName);
            TailPolygon.SetValue(FrameworkElement.StyleProperty, style);
        }

        private void RefreshTailSizes()
        {
            if (!(PlacementTarget is FrameworkElement placementTargetControl))
            {
                return;
            }

            var placementMode = Placement;

            switch (placementMode)
            {
                case PlacementMode.Bottom:

                    Popup.SetCurrentValue(Popup.HorizontalOffsetProperty, (placementTargetControl.ActualWidth - ContentBorder.ActualWidth) / 2d + HorizontalOffset);
                    Popup.SetCurrentValue(Popup.VerticalOffsetProperty,  -8d);

                    BorderGapRectangle.SetCurrentValue(WidthProperty, TailBaseWidth - 4);
                    BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Top);

                    break;


                case PlacementMode.Top:

                    Popup.SetCurrentValue(Popup.VerticalOffsetProperty, 8d);
                    Popup.SetCurrentValue(Popup.HorizontalOffsetProperty, (placementTargetControl.ActualWidth - ContentBorder.ActualWidth) / 2d + HorizontalOffset);

                    BorderGapRectangle.SetCurrentValue(WidthProperty, TailBaseWidth - 4);
                    BorderGapRectangle.SetCurrentValue(HeightProperty, 2d);
                    BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);

                    break;

                case PlacementMode.Right:
                    Popup.SetCurrentValue(Popup.VerticalOffsetProperty, (placementTargetControl.ActualHeight - ContentBorder.ActualHeight) / 2d + VerticalOffset);
                    Popup.SetCurrentValue(Popup.HorizontalOffsetProperty, -8d);

                    BorderGapRectangle.SetCurrentValue(WidthProperty, 2d);
                    BorderGapRectangle.SetCurrentValue(HeightProperty, TailBaseWidth - 4);
                    BorderGapRectangle.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);

                    break;



                case PlacementMode.Left:
                    Popup.SetCurrentValue(Popup.VerticalOffsetProperty, (placementTargetControl.ActualHeight - ContentBorder.ActualHeight) / 2d + VerticalOffset);
                    Popup.SetCurrentValue(Popup.HorizontalOffsetProperty, 8d);

                    break;

                default:
                    //NOTE:Vladimir:Will throw if not supported
                    throw new NotSupportedException($"Callout placement = '{placementMode}' not supported. Supported modes: '{PlacementMode.Left}', '{PlacementMode.Top}', '{PlacementMode.Right}', '{PlacementMode.Bottom}'");
            }
        }

        private string GetTailPolygonStyleResourceName()
        {
            var tailAlignment = Placement == PlacementMode.Left || Placement == PlacementMode.Right ? (object) TailVerticalAlignment : TailHorizontalAlignment;

            return $"{Placement}{tailAlignment}PolygonStyle";
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
           // UpdatePopupPosition();
        }
        
        private void OnPlacementChanged(DependencyPropertyChangedEventArgs args)
        {
            //UpdatePopupPosition();
        }
    }
}
