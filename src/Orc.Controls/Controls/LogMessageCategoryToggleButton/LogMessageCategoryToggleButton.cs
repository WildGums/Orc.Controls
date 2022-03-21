namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using Automation;
    using Converters;

    [TemplatePart(Name = "PART_Toggle", Type = typeof(ToggleButton))]
    public class LogMessageCategoryToggleButton : Control
    {
        #region Fields
        private ToggleButton _toggleButton;
        #endregion

        #region Dependency properties
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked),
            typeof(bool), typeof(LogMessageCategoryToggleButton), new FrameworkPropertyMetadata(false, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public int EntryCount
        {
            get { return (int)GetValue(EntryCountProperty); }
            set { SetValue(EntryCountProperty, value); }
        }

        public static readonly DependencyProperty EntryCountProperty = DependencyProperty.Register(
            nameof(EntryCount), typeof(int), typeof(LogMessageCategoryToggleButton), new PropertyMetadata(default(int)));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(nameof(Category),
            typeof(string), typeof(LogMessageCategoryToggleButton), new PropertyMetadata("",
                (sender, args) => ((LogMessageCategoryToggleButton)sender).OnCategoryChanged(args)));
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toggleButton = (ToggleButton)GetTemplateChild("PART_Toggle");
        }

        private void OnCategoryChanged(DependencyPropertyChangedEventArgs args)
        {
            var brushes = LogMessageCategoryBorderBrushConverter.BrushCache;

            var newBrushName = args.NewValue as string;
            if (string.IsNullOrWhiteSpace(newBrushName))
            {
                return;
            }

            if (!brushes.TryGetValue(newBrushName, out var brush))
            {
                return;
            }

            var accentColor = brush.Color;
            var accentColor20 = Color.FromArgb(51, accentColor.R, accentColor.G, accentColor.B);
            var accentColor40 = Color.FromArgb(102, accentColor.R, accentColor.G, accentColor.B);
            var accentColor60 = Color.FromArgb(153, accentColor.R, accentColor.G, accentColor.B);

            var accentColorBrush = new SolidColorBrush(accentColor);
            var accentColorBrush20 = new SolidColorBrush(accentColor20);
            var accentColorBrush40 = new SolidColorBrush(accentColor40);
            var accentColorBrush60 = new SolidColorBrush(accentColor60);

            Resources.Add("Orc.Brushes.Control.Default.Border", accentColorBrush40);

            Resources.Add("Orc.Brushes.Control.MouseOver.Background", accentColorBrush20);
            Resources.Add("Orc.Brushes.Control.MouseOver.Border", accentColorBrush40);
            Resources.Add("Orc.Brushes.Control.Pressed.Background", accentColorBrush40);
            Resources.Add("Orc.Brushes.Control.Pressed.Border", accentColorBrush60);
            Resources.Add("Orc.Brushes.Control.Disabled.Background", accentColorBrush20);
            Resources.Add("Orc.Brushes.Control.Checked.Background", accentColorBrush);
            Resources.Add("Orc.Brushes.AccentColor20", accentColorBrush20);
            Resources.Add("Orc.Brushes.AccentColor40", accentColorBrush40);
        }

        public void Toggle()
        {
            _toggleButton.Toggle();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new LogMessageCategoryToggleButtonModelPeer(this);
        }
    }
}
