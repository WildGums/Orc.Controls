namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Automation;

    /// <summary>
    /// A counter to show the frame rate inside an application.
    /// </summary>
    public class FrameRateCounter : TextBlock
    {
        private readonly DispatcherTimer _frameRateTimer = new DispatcherTimer();

        private int _frameRateCounter;

        // Note: we cache dependency properties for performance
        private string _prefixThreadSafe;

        static FrameRateCounter()
        {
            //FrameRateCounter.FontFamilyProperty.OverrideMetadata(typeof(FrameRateCounter), new PropertyMetadata(new FontFamily("Consolas")));
        }

        public FrameRateCounter()
        {
            Loaded += OnControlLoaded;
            Unloaded += OnControlUnloaded;

            _prefixThreadSafe = Prefix;

            _frameRateTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _frameRateTimer.Tick += (sender, e) => OnFrameRateCounterElapsed();
        }

        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(nameof(Prefix),
            typeof(string), typeof(FrameRateCounter), new PropertyMetadata("Frame rate: ", (sender, e) => ((FrameRateCounter)sender)._prefixThreadSafe = (string)e.NewValue));

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _frameRateTimer.Start();

            CompositionTarget.Rendering += OnRendering;
        }

        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;

            _frameRateTimer.Stop();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _frameRateCounter++;
        }

        private void OnFrameRateCounterElapsed()
        {
            Update();

            _frameRateCounter = 0;
        }

        private void Update()
        {
            var text = $"{_prefixThreadSafe}{_frameRateCounter}";

            SetCurrentValue(TextProperty, text);
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameRateCounterAutomationPeer(this);
        }
    }
}
