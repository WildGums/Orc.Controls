namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Catel.IoC;
using Catel.Logging;
using Catel.MVVM.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[ContentProperty(nameof(InnerContent))]
public partial class Callout
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(Callout));

    private const double DropShadowSize = 2d;
    private const double ContentBorderPadding = 8d;

    static Callout()
    {
        typeof(Callout).AutoDetectViewPropertiesToSubscribe(IoCContainer.ServiceProvider.GetRequiredService<IViewPropertySelector>());
    }

    partial void OnInitializedComponent()
    {
        Popup.Opened += PopupOnOpened;
    }

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public object? InnerContent
    {
        get { return GetValue(InnerContentProperty); }
        set { SetValue(InnerContentProperty, value); }
    }

    public static readonly DependencyProperty InnerContentProperty =
        DependencyProperty.Register(nameof(InnerContent), typeof(object), typeof(Callout));


    [ViewToViewModel(viewModelPropertyName: nameof(CalloutViewModel.Name), MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string? CalloutName
    {
        get { return (string?)GetValue(CalloutNameProperty); }
        set { SetValue(CalloutNameProperty, value); }
    }

    public static readonly DependencyProperty CalloutNameProperty = DependencyProperty.Register(nameof(CalloutName),
        typeof(string), typeof(Callout), new PropertyMetadata(null));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string? Title
    {
        get { return (string?)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(Callout));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string? Description
    {
        get { return (string?)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(Callout));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public object? PlacementTarget
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
    public ICommand? Command
    {
        get { return (ICommand?)GetValue(CommandProperty); }
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
        if (PlacementTarget is not FrameworkElement placementTarget)
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
                y += ContentBorderPadding - 1;
                break;

            case PlacementMode.Right:
                x += targetSizeHalfWidth;
                x += popupHalfWidth;
                y += ContentBorderPadding - 1;
                break;

            default:
                throw Logger.LogErrorAndCreateException<NotSupportedException>($"Callout placement = '{Placement}' not supported. Supported modes: '{PlacementMode.Left}', '{PlacementMode.Top}', '{PlacementMode.Right}', '{PlacementMode.Bottom}'");
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


    private void PopupOnOpened(object? sender, EventArgs e)
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

        var showTailBorderGap = false;

        if (placementMode is PlacementMode.Left or PlacementMode.Right)
        {
            showTailBorderGap = HorizontalOffset != 0 && TailBaseWidth != 0;
            TailPolygon.SetCurrentValue(FrameworkElement.WidthProperty, HorizontalOffset);
            BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, TailVerticalAlignment);
            BorderGapRectangle.SetCurrentValue(WidthProperty, 3d);
            BorderGapRectangle.SetCurrentValue(HeightProperty, TailBaseWidth - 3);
        }

        if (placementMode == PlacementMode.Top || placementMode == PlacementMode.Bottom)
        {
            showTailBorderGap = VerticalOffset != 0 && TailBaseWidth != 0;
            TailPolygon.SetCurrentValue(FrameworkElement.HeightProperty, VerticalOffset);
            BorderGapRectangle.SetCurrentValue(HorizontalAlignmentProperty, TailHorizontalAlignment);
            BorderGapRectangle.SetCurrentValue(WidthProperty, TailBaseWidth - 3);
            BorderGapRectangle.SetCurrentValue(HeightProperty, 2d);
        }

        // Don't create gap if no tail present
        if (!showTailBorderGap)
        {
            BorderGapRectangle.SetCurrentValue(WidthProperty, 0d);
            BorderGapRectangle.SetCurrentValue(HeightProperty, 0d);
            return;
        }

        Thickness margin;

        // Locate border gap having in mind margins of Tail
        switch (placementMode)
        {
            case PlacementMode.Top:
                BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
                margin = TailPolygon.Margin;
                BorderGapRectangle.SetCurrentValue(MarginProperty, GetHorizontalGapMargin(margin, placementMode, TailHorizontalAlignment));
                break;

            case PlacementMode.Bottom:
                BorderGapRectangle.SetCurrentValue(VerticalAlignmentProperty, VerticalAlignment.Top);
                margin = TailPolygon.Margin;
                BorderGapRectangle.SetCurrentValue(MarginProperty, GetHorizontalGapMargin(margin, placementMode, TailHorizontalAlignment));
                break;

            case PlacementMode.Left:
                BorderGapRectangle.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
                margin = TailPolygon.Margin;
                BorderGapRectangle.SetCurrentValue(MarginProperty, GetVerticalGapMargin(margin, placementMode, TailVerticalAlignment));
                break;

            case PlacementMode.Right:
                BorderGapRectangle.SetCurrentValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
                margin = TailPolygon.Margin;
                BorderGapRectangle.SetCurrentValue(MarginProperty, GetVerticalGapMargin(margin, placementMode, TailVerticalAlignment));
                break;
        }
    }

    private Thickness GetHorizontalGapMargin(Thickness tailPolygonMargin, PlacementMode placementMode, HorizontalAlignment horizontalAlignment)
    {
        if (horizontalAlignment == HorizontalAlignment.Right)
        {
            return new Thickness(0, 0, tailPolygonMargin.Right + 2, 0);
        }

        // Increase top margin for top placement;
        var topMarginThickness = tailPolygonMargin.Top + (placementMode == PlacementMode.Top ? 1d : 0d);

        if (horizontalAlignment == HorizontalAlignment.Center)
        {
            return new Thickness(tailPolygonMargin.Left, topMarginThickness, 0, 0);
        }

        if (horizontalAlignment == HorizontalAlignment.Left)
        {
            return new Thickness(tailPolygonMargin.Left + 1, topMarginThickness, 0, 0);
        }

        return new Thickness(0);
    }


    private Thickness GetVerticalGapMargin(Thickness tailPolygonMargin, PlacementMode placementMode, VerticalAlignment verticalAlignment)
    {
        return verticalAlignment switch
        {
            VerticalAlignment.Top or VerticalAlignment.Center => new Thickness(tailPolygonMargin.Left, tailPolygonMargin.Top + 1, 0, 0),
            VerticalAlignment.Bottom => new Thickness(tailPolygonMargin.Left, tailPolygonMargin.Top, tailPolygonMargin.Right, tailPolygonMargin.Bottom + 2),
            _ => new Thickness(0)
        };
    }

    private string GetTailPolygonStyleResourceName()
    {
        var tailAlignment = Placement is PlacementMode.Left or PlacementMode.Right 
            ? (object)TailVerticalAlignment 
            : TailHorizontalAlignment;

        return $"{Placement}{tailAlignment}PolygonStyle";
    }
}
