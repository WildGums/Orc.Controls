// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameRateCounter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// A counter to show the frame rate inside an application.
    /// </summary>
    public class FrameRateCounter : TextBlock
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRateCounter"/> class.
        /// </summary>
        public FrameRateCounter()
        {
            Loaded += OnControlLoaded;
            Unloaded += OnControlUnloaded;

            _frameRateTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _frameRateTimer.Tick += (sender, e) => OnFrameRateCounterElapsed();
        }
        #endregion

        #region Fields
        private int _frameRateCounter;
        private readonly DispatcherTimer _frameRateTimer = new DispatcherTimer();
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix
        {
            get { return (string)GetValue(PrefixProperty); }
            set { SetValue(PrefixProperty, value); }
        }

        /// <summary>
        /// The prefix dependency property definition.
        /// </summary>
        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(nameof(Prefix),
            typeof(string), typeof(FrameRateCounter), new PropertyMetadata("Frame rate: "));
        #endregion

        #region Methods
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
            var text = string.Empty;
            if (!string.IsNullOrWhiteSpace(Prefix))
            {
                text += Prefix;
            }

            text += _frameRateCounter.ToString();

            SetCurrentValue(TextProperty, text);

            _frameRateCounter = 0;
        }
        #endregion
    }
}
