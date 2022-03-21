namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Shapes;
    using Automation;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Size = System.Windows.Size;


    [TemplatePart(Name = "PART_Arrow", Type = typeof(Path))]
    [TemplatePart(Name = "PART_ArrowBorder", Type = typeof(Border))]
    public class DropDownButton : ToggleButton
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDispatcherService _dispatcherService;

        private Path _arrowPath;
        private Border _arrowBorder;
        #endregion

        #region Constructors
        public DropDownButton()
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            _dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        }
        #endregion

        #region Dependency properties
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(object), typeof(DropDownButton),
            new PropertyMetadata(default(object), (sender, args) => ((DropDownButton)sender).OnHeaderChanged()));

        public DropdownArrowLocation ArrowLocation
        {
            get { return (DropdownArrowLocation)GetValue(ArrowLocationProperty); }
            set { SetValue(ArrowLocationProperty, value); }
        }

        public static readonly DependencyProperty ArrowLocationProperty = DependencyProperty.Register(nameof(ArrowLocation),
            typeof(DropdownArrowLocation), typeof(DropDownButton), new PropertyMetadata(DropdownArrowLocation.Right));

        public Thickness ArrowMargin
        {
            get { return (Thickness)GetValue(ArrowMarginProperty); }
            set { SetValue(ArrowMarginProperty, value); }
        }

        public static readonly DependencyProperty ArrowMarginProperty = DependencyProperty.Register(nameof(ArrowMargin), 
            typeof(Thickness), typeof(DropDownButton), new PropertyMetadata(default(Thickness)));

        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register(nameof(DropDown), typeof(ContextMenu),
            typeof(DropDownButton), new UIPropertyMetadata(null, (sender, args) => ((DropDownButton)sender).OnDropDownChanged(args)));

        public bool IsArrowVisible
        {
            get { return (bool)GetValue(IsArrowVisibleProperty); }
            set { SetValue(IsArrowVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsArrowVisibleProperty = DependencyProperty.Register(nameof(IsArrowVisible),
            typeof(bool), typeof(DropDownButton), new PropertyMetadata(true));
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _arrowPath = GetTemplateChild("PART_Arrow") as Path;
            if (_arrowPath is null)
            {
               throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Arrow'");
            }

            _arrowBorder = GetTemplateChild("PART_ArrowBorder") as Border;
            if (_arrowBorder is null)
            {
               throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ArrowBorder'");
            }
        }
        private void OnHeaderChanged()
        {
            SetCurrentValue(ContentProperty, Header);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            SetCurrentValue(HeaderProperty, Content);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (Command is null)
            {
                return;
            }

            if (Equals(e.OriginalSource, _arrowPath) || Equals(e.OriginalSource, _arrowBorder))
            {
                SetCurrentValue(IsCheckedProperty, !IsChecked);

                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            var source = e.OriginalSource;
        }

        private void OnDropDownChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue is ContextMenu oldDropDownContextMenu)
            {
                oldDropDownContextMenu.Closed -= OnDropDownContextMenuClosed;
            }

            if (args.NewValue is ContextMenu newDropDownContextMenu)
            {
                newDropDownContextMenu.Closed += OnDropDownContextMenuClosed;
            }
        }

        private void OnDropDownContextMenuClosed(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(IsCheckedProperty, false);
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);

            var dropDown = DropDown;
            if (dropDown is null)
            {
                return;
            }

            _dispatcherService.Invoke(() =>
            {
                dropDown.SetCurrentValue(ContextMenu.PlacementTargetProperty, this);
                dropDown.SetCurrentValue(ContextMenu.PlacementProperty, PlacementMode.Custom);
                dropDown.SetCurrentValue(ContextMenu.MinWidthProperty, ActualWidth);
                dropDown.SetCurrentValue(ContextMenu.CustomPopupPlacementCallbackProperty, (CustomPopupPlacementCallback)CustomPopupPlacementCallback);
            });

            dropDown.SetCurrentValue(ContextMenu.IsOpenProperty, true);
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);

            DropDown?.SetCurrentValue(ContextMenu.IsOpenProperty, false);
        }

        private static CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            var p = new Point
            {
                Y = targetSize.Height - offset.Y,
                X = -offset.X
            };

            return new[]
            {
                new CustomPopupPlacement(p, PopupPrimaryAxis.Horizontal)
            };
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new DropDownButtonAutomationPeer(this);
        }
        #endregion
    }
}
