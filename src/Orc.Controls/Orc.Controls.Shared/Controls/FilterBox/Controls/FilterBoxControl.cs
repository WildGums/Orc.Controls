// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using Catel.MVVM;
    using Catel.Windows.Interactivity;

    [TemplatePart(Name = "PART_FilterTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class FilterBoxControl : ContentControl
    {
        private readonly Command _clearFilter;
        private Button _clearButton;
        private TextBox _filterTextBox;

        public event EventHandler<InitializingAutoCompletionServiceEventArgs> InitializingAutoCompletionService;

        public FilterBoxControl()
        {
            _clearFilter = new Command(OnClearFilter, CanClearFilter);
        }

        #region Properties
        public IEnumerable FilterSource
        {
            get { return (IEnumerable) GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register("FilterSource", typeof(IEnumerable),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(null));

        public string PropertyName
        {
            get { return (string) GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FilterBoxControl),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterBoxControl) sender).OnTextChanged(sender, e)));

        public Brush AccentColorBrush
        {
            get { return (Brush) GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((FilterBoxControl) sender).OnAccentColorBrushChanged()));

        public string Watermark
        {
            get { return (string) GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Methods
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            _filterTextBox?.Focus();
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("FilterBox");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);

            if (_clearButton != null)
            {
                _clearButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, null);
            }

            _clearButton = (Button) GetTemplateChild("PART_ClearButton");
            if (_clearButton != null)
            {
                _clearButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _clearFilter);
            }

            _filterTextBox = (TextBox) GetTemplateChild("PART_FilterTextBox");


            var serviceEventArg = new InitializingAutoCompletionServiceEventArgs();
            OnInitializingAutoCompletionService(serviceEventArg);
            var autoCompletionService = serviceEventArg.AutoCompletionService;

            if (autoCompletionService != null)
            {
                var autoCompletion = Interaction.GetBehaviors(_filterTextBox).OfType<AutoCompletion>().FirstOrDefault();
                if (autoCompletion != null)
                {
                    var autoCompletionServiceFieldInfo = autoCompletion.GetType().GetField("_autoCompletionService", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (autoCompletionServiceFieldInfo != null)
                    {
                        autoCompletionServiceFieldInfo.SetValue(autoCompletion, autoCompletionService);
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsEnabled || e.Handled || (e.Key != Key.Escape || !CanClearFilter()))
            {
                return;
            }

            OnClearFilter();
            e.Handled = true;
        }

        private void OnClearFilter()
        {
            SetCurrentValue(TextProperty, string.Empty);
        }

        private bool CanClearFilter()
        {
            return !string.IsNullOrWhiteSpace(Text);
        }

        private void OnTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            _clearFilter.RaiseCanExecuteChanged();
        }

        protected virtual void OnInitializingAutoCompletionService(InitializingAutoCompletionServiceEventArgs e)
        {
            InitializingAutoCompletionService?.Invoke(this, e);
        }

        #endregion
    }
}