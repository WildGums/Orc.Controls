namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Catel;

    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    public class SpinButton : Control
    {
        static SpinButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinButton), new FrameworkPropertyMetadata(typeof(SpinButton)));
        }

        public static RoutedUICommand CancelChangesCommand { get; set; } = new("CancelChanges", "CancelChanges", typeof(SpinButton));
        public static readonly RoutedUICommand MajorDecreaseValueCommand = new("MajorDecreaseValue", "MajorDecreaseValue", typeof(SpinButton));
        public static readonly RoutedUICommand MajorIncreaseValueCommand = new("MajorIncreaseValue", "MajorIncreaseValue", typeof(SpinButton));
        public static readonly RoutedUICommand MinorDecreaseValueCommand = new("MinorDecreaseValue", "MinorDecreaseValue", typeof(SpinButton));
        public static readonly RoutedUICommand MinorIncreaseValueCommand = new("MinorIncreaseValue", "MinorIncreaseValue", typeof(SpinButton));
        public static readonly RoutedUICommand UpdateValueStringCommand = new("UpdateValueString", "UpdateValueString", typeof(SpinButton));

        private RepeatButton _decreaseButton;
        private RepeatButton _increaseButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachIncreaseButton();
            AttachDecreaseButton();
        }


        private void AttachIncreaseButton()
        {
            var increaseButton = GetTemplateChild("PART_IncreaseButton") as RepeatButton;
            if (increaseButton is null)
            {
                return;
            }

            _increaseButton = increaseButton;
            _increaseButton.SetCurrentValue(FocusableProperty, false);
            _increaseButton.SetCurrentValue(ButtonBase.CommandProperty, MinorIncreaseValueCommand);
            _increaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            _increaseButton.PreviewMouseRightButtonDown += ButtonOnPreviewMouseRightButtonDown;
        }

        private void AttachDecreaseButton()
        {
            var decreaseButton = GetTemplateChild("PART_DecreaseButton") as RepeatButton;
            if (decreaseButton is null)
            {
                return;
            }

            _decreaseButton = decreaseButton;
            _decreaseButton.SetCurrentValue(FocusableProperty, false);
            _decreaseButton.SetCurrentValue(ButtonBase.CommandProperty, MinorDecreaseValueCommand);
            _decreaseButton.PreviewMouseLeftButtonDown += (sender, args) => RemoveFocus();
            _decreaseButton.PreviewMouseRightButtonDown += ButtonOnPreviewMouseRightButtonDown;

        }

        private void RemoveFocus()
        {
            SetCurrentValue(FocusableProperty, true);
            Focus();
            SetCurrentValue(FocusableProperty, false);
        }

        private void ButtonOnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetCurrentValue(SpinButtonProperty, (decimal)0);
        }

        private static void OnSpinButtonChanged(DependencyObject element,
            DependencyPropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty SpinButtonProperty = DependencyProperty.Register(nameof(SpinButton), typeof(decimal), typeof(SpinButton),
           new PropertyMetadata(0m, OnSpinButtonChanged));
    }
}
