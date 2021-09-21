namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel.Logging;

    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class SpinButton : Control
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private RepeatButton _decreaseButton;
        private RepeatButton _increaseButton;

        static SpinButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinButton), new FrameworkPropertyMetadata(typeof(SpinButton)));

            CommandManager.RegisterClassInputBinding(typeof(SpinButton), new KeyBinding(MinorDecrease, new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(typeof(SpinButton), new KeyBinding(MinorIncrease, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(SpinButton), new KeyBinding(MajorIncrease, new KeyGesture(Key.PageUp)));
            CommandManager.RegisterClassInputBinding(typeof(SpinButton), new KeyBinding(MajorDecrease, new KeyGesture(Key.PageDown)));
        }

        #region Routed Events
        public static readonly RoutedEvent CanceledEvent = EventManager.RegisterRoutedEvent(nameof(Canceled), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(SpinButton));

        public event RoutedEventHandler Canceled
        {
            add { AddHandler(CanceledEvent, value); }
            remove { RemoveHandler(CanceledEvent, value); }
        }
        #endregion

        #region Routed commands
        public static RoutedCommand MajorDecrease { get; } = new (nameof(MajorDecrease), typeof(SpinButton));
        public static RoutedCommand MajorIncrease { get; } = new (nameof(MajorIncrease), typeof(SpinButton));
        public static RoutedCommand MinorDecrease { get; } = new (nameof(MinorDecrease), typeof(SpinButton));
        public static RoutedCommand MinorIncrease { get; } = new (nameof(MinorIncrease), typeof(SpinButton));
        #endregion

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

            _decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (_decreaseButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_DecreaseButton'");
            }
        }
    }
}
