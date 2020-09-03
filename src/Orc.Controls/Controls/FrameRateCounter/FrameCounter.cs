namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// A counter to show the frame number inside an application.
    /// </summary>
    public class FrameCounter : TextBlock
    {
        private int _frameCounter;

        // Note: we cache dependency properties for performance
        private int _resetCount;
        private string _prefix;

        static FrameCounter()
        {
            //FrameCounter.FontFamilyProperty.OverrideMetadata(typeof(FrameCounter), new PropertyMetadata(new FontFamily("Consolas")));
        }

        public FrameCounter()
        {
            Loaded += OnControlLoaded;
            Unloaded += OnControlUnloaded;

            _resetCount = ResetCount;
            _prefix = Prefix;   
        }

        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(nameof(Prefix),
            typeof(string), typeof(FrameCounter), new PropertyMetadata("Frame no: ", (sender, e) => ((FrameCounter)sender)._prefix = (string)e.NewValue));


        public int ResetCount
        {
            get { return (int)GetValue(ResetCountProperty); }
            set { SetValue(ResetCountProperty, value); }
        }

        public static readonly DependencyProperty ResetCountProperty = DependencyProperty.Register(nameof(ResetCount), 
            typeof(int), typeof(FrameCounter), new PropertyMetadata(1000, (sender, e) => ((FrameCounter)sender)._resetCount = (int)e.NewValue));


        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            _frameCounter++;

            if (_frameCounter > _resetCount)
            {
                _frameCounter = 0;
            }

            Update();
        }

        private void Update()
        {
            var text = $"{_prefix}{_frameCounter}";

            SetCurrentValue(TextProperty, text);
        }
    }
}
