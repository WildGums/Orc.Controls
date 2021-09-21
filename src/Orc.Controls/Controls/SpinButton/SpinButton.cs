namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls.Primitives;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using Catel.Logging;
    using Theming;
    using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
    using Control = System.Windows.Controls.Control;
    using KeyEventArgs = System.Windows.Input.KeyEventArgs;

    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class SpinButton : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private RepeatButton _decreaseButton;
        private RepeatButton _increaseButton;

        private Storyboard _decreaseButtonStoryboard;
        private Storyboard _increaseButtonStoryboard;

        private RepeatButtonAutomationPeer _decreaseButtonAutomationPeer;
        private RepeatButtonAutomationPeer _increaseButtonAutomationPeer;

        static SpinButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinButton), new FrameworkPropertyMetadata(typeof(SpinButton)));
        }

        public SpinButton()
        {
            Increased += OnIncreased;
            Decreased += OnDecreased;
        }

        #region Depencency properties
        public ICommand Increase
        {
            get { return (ICommand)GetValue(IncreaseProperty); }
            set { SetValue(IncreaseProperty, value); }
        }

        public static readonly DependencyProperty IncreaseProperty = DependencyProperty.Register(
            nameof(Increase), typeof(ICommand), typeof(SpinButton), new PropertyMetadata(default(ICommand)));

        public ICommand Decrease
        {
            get { return (ICommand)GetValue(DecreaseProperty); }
            set { SetValue(DecreaseProperty, value); }
        }

        public static readonly DependencyProperty DecreaseProperty = DependencyProperty.Register(
            nameof(Decrease), typeof(ICommand), typeof(SpinButton), new PropertyMetadata(default(ICommand)));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(SpinButton), new PropertyMetadata(default(object)));
        #endregion

        #region Routed Events
        public static readonly RoutedEvent CanceledEvent = EventManager.RegisterRoutedEvent(nameof(Canceled), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SpinButton));

        public event RoutedEventHandler Canceled
        {
            add { AddHandler(CanceledEvent, value); }
            remove { RemoveHandler(CanceledEvent, value); }
        }

        public static readonly RoutedEvent IncreasedEvent = EventManager.RegisterRoutedEvent(nameof(Increased), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SpinButton));

        public event RoutedEventHandler Increased
        {
            add { AddHandler(IncreasedEvent, value); }
            remove { RemoveHandler(IncreasedEvent, value); }
        }

        public static readonly RoutedEvent DecreasedEvent = EventManager.RegisterRoutedEvent(nameof(Decreased), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SpinButton));

        public event RoutedEventHandler Decreased
        {
            add { AddHandler(DecreasedEvent, value); }
            remove { RemoveHandler(DecreasedEvent, value); }
        }
        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Up && _increaseButton.IsEnabled)
            {
                _increaseButtonAutomationPeer ??= new RepeatButtonAutomationPeer(_increaseButton);
                var invokeProv = _increaseButtonAutomationPeer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv?.Invoke();

                return;
            }

            if (e.Key == Key.Down && _decreaseButton.IsEnabled)
            {
                _decreaseButtonAutomationPeer ??= new RepeatButtonAutomationPeer(_decreaseButton);
                var invokeProv = _decreaseButtonAutomationPeer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv?.Invoke();

                return;
            }
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CanceledEvent));
        }

        public override void OnApplyTemplate()
        {
            _increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (_increaseButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_IncreaseButton'");
            }
            _increaseButton.Click += OnIncreaseButtonClick;

            _decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (_decreaseButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DecreaseButton'");
            }
            _decreaseButton.Click += OnDecreaseButtonClick;
        }

        private void OnIncreaseButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(IncreasedEvent));
        }

        private void OnDecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DecreasedEvent));
        }
        
        private void OnIncreased(object sender, RoutedEventArgs e)
        {
            _increaseButtonStoryboard ??= CreateHighlightAnimation(_increaseButton);
            _increaseButtonStoryboard.Begin();
        }

        private void OnDecreased(object sender, RoutedEventArgs e)
        {
            _decreaseButtonStoryboard ??= CreateHighlightAnimation(_decreaseButton);
            _decreaseButtonStoryboard.Begin();
        }

        private Storyboard CreateHighlightAnimation(RepeatButton button)
        {
            var sb = new Storyboard();

            var animation = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                To = ThemeManager.Current.GetThemeColor(),
                FillBehavior = FillBehavior.Stop
            };

            // Set the target of the animation
            Storyboard.SetTarget(animation, button);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Button.Background).(SolidColorBrush.Color)"));

            // Kick the animation off
            sb.Children.Add(animation);

            return sb;
        }
    }
}
