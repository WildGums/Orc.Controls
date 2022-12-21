namespace Orc.Controls
{
    using System;
    using System.Windows.Threading;
    using Catel;

    /// <summary>
    /// A timer used to open and close tooltips.
    /// </summary>
    internal sealed class ToolTipTimer : DispatcherTimer
    {
        #region Constants
        /// <summary>
        /// The timer interval.
        /// </summary>
        private const int TimerInterval = 100;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTipTimer" /> class.
        /// </summary>
        /// <param name="maximumTicks">The maximum ticks.</param>
        /// <param name="initialDelay">The initial delay.</param>
        public ToolTipTimer(TimeSpan maximumTicks, TimeSpan initialDelay)
        {
            InitialDelay = initialDelay;
            MaximumTicks = maximumTicks;
            Interval = TimeSpan.FromMilliseconds(TimerInterval);
            Tick += OnTick;
        }
        #endregion

        #region Public Events
        /// <summary>
        /// This event occurs when the timer has stopped.
        /// </summary>
        public event EventHandler Stopped;
        #endregion

        #region Methods
        private void OnTick(object sender, EventArgs e)
        {
            CurrentTick += TimerInterval;

            if (CurrentTick >= (MaximumTicks.TotalMilliseconds + InitialDelay.TotalMilliseconds))
            {
                Stop();
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the current tick of the ToolTipTimer.
        /// </summary>
        public int CurrentTick { get; private set; }

        /// <summary>
        /// Gets the initial delay for this timer in seconds.
        /// When the maximum number of ticks is hit, the timer will stop itself.
        /// </summary>
        /// <remarks>The default delay is 0 seconds.</remarks>
        public TimeSpan InitialDelay { get; set; }

        /// <summary>
        /// Gets the maximum number of seconds for this timer.
        /// When the maximum number of ticks is hit, the timer will stop itself.
        /// </summary>
        /// <remarks>The minimum number of seconds is 1.</remarks>
        public TimeSpan MaximumTicks { get; set; }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Resets the ToolTipTimer and starts it.
        /// </summary>
        public void StartAndReset()
        {
            CurrentTick = 0;
            Start();
        }

        /// <summary>
        /// Stops the ToolTipTimer.
        /// </summary>
        public new void Stop()
        {
            base.Stop();

            Stopped?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Stops the ToolTipTimer and resets its tick count.
        /// </summary>
        public void StopAndReset()
        {
            Stop();
            CurrentTick = 0;
        }
        #endregion
    }
}
