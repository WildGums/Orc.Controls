// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusyIndicator.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows.Threading;

    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator
    {
        #region Fields
        private MediaElementThreadInfo _mediaElementThreadInfo;
        private Grid _grid;

        private Brush _foreground;
        private FluidProgressBar _fluidProgressBar;
        private int _ignoreUnloadedEventCount;

        private readonly IDispatcherService _dispatcherService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyIndicator"/> class.
        /// </summary>
        public BusyIndicator()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            _dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
        }
        #endregion

        #region Properties
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            nameof(Foreground), typeof(Brush), typeof(BusyIndicator),
            new PropertyMetadata(Brushes.White, (sender, args) => ((BusyIndicator)sender).OnForegroundChanged(args)));

        public int IgnoreUnloadedEventCount
        {
            get { return (int)GetValue(IgnoreUnloadedEventCountProperty); }
            set { SetValue(IgnoreUnloadedEventCountProperty, value); }
        }

        public static readonly DependencyProperty IgnoreUnloadedEventCountProperty = DependencyProperty.Register(nameof(IgnoreUnloadedEventCount),
            typeof(int), typeof(BusyIndicator), new PropertyMetadata(0, (sender, e) => ((BusyIndicator)sender).OnIgnoreUnloadedEventCountChanged()));
        #endregion

        #region Methods
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_mediaElementThreadInfo != null)
            {
                return;
            }

            _mediaElementThreadInfo = MediaElementThreadFactory.CreateMediaElementsOnWorkerThread(CreateBusyIndicator);
            Child = _mediaElementThreadInfo.HostVisual;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_ignoreUnloadedEventCount > 0)
            {
                _ignoreUnloadedEventCount--;
                return;
            }

            if (_mediaElementThreadInfo != null)
            {
                _mediaElementThreadInfo.Dispose();
                _mediaElementThreadInfo = null;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_grid != null && sizeInfo.WidthChanged)
            {
                _grid.Dispatcher.BeginInvoke(() =>
                {
                    _grid.SetCurrentValue(WidthProperty, sizeInfo.NewSize.Width);
                });
            }
        }

        private void OnIgnoreUnloadedEventCountChanged()
        {
            _ignoreUnloadedEventCount = IgnoreUnloadedEventCount;
        }

        private void OnForegroundChanged(DependencyPropertyChangedEventArgs args)
        {
            _foreground = Foreground;

            _mediaElementThreadInfo?.Dispatcher.Invoke(() =>
            {
                _fluidProgressBar.Foreground = _foreground;
            });
        }

        private FrameworkElement CreateBusyIndicator()
        {
            _fluidProgressBar = new FluidProgressBar
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Foreground = _foreground
            };

            var grid = new Grid
            {
                Width = ActualWidth,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            grid.Children.Add(_fluidProgressBar);

            _grid = grid;

            return grid;
        }
        #endregion
    }
}
