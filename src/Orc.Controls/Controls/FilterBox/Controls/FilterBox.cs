// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
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
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Windows.Interactivity;

    [TemplatePart(Name = "PART_FilterTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class FilterBox : ContentControl
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Command _clearFilter;
        private Button _clearButton;
        private TextBox _filterTextBox;

        public event EventHandler<InitializingAutoCompletionServiceEventArgs> InitializingAutoCompletionService;

        public FilterBox()
        {
            _clearFilter = new Command(OnClearFilter, CanClearFilter);
        }

        #region Properties
        public bool AllowAutoCompletion
        {
            get { return (bool)GetValue(AllowAutoCompletionProperty); }
            set { SetValue(AllowAutoCompletionProperty, value); }
        }

        public static readonly DependencyProperty AllowAutoCompletionProperty = DependencyProperty.Register(
            nameof(AllowAutoCompletion), typeof(bool), typeof(FilterBox), new PropertyMetadata(true));

        public IEnumerable FilterSource
        {
            get { return (IEnumerable)GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register(nameof(FilterSource), typeof(IEnumerable),
            typeof(FilterBox), new FrameworkPropertyMetadata(null));

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string),
            typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(FilterBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((FilterBox)sender).OnTextChanged()));


        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string),
            typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Methods
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            _filterTextBox?.Focus();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clearButton?.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, null);

            _clearButton = (Button)GetTemplateChild("PART_ClearButton");
            _clearButton?.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _clearFilter);

            _filterTextBox = GetTemplateChild("PART_FilterTextBox") as TextBox;
            if (_filterTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_FilterTextBox'");
            }

            var serviceEventArg = new InitializingAutoCompletionServiceEventArgs();
            OnInitializingAutoCompletionService(serviceEventArg);
            var autoCompletionService = serviceEventArg.AutoCompletionService;

            if (autoCompletionService == null)
            {
                return;
            }

            var autoCompletion = Interaction.GetBehaviors(_filterTextBox).OfType<AutoCompletion>().FirstOrDefault();
            if (autoCompletion == null)
            {
                return;
            }

            var autoCompletionServiceFieldInfo = autoCompletion.GetType().GetField("_autoCompletionService", BindingFlags.Instance | BindingFlags.NonPublic);
            if (autoCompletionServiceFieldInfo != null)
            {
                autoCompletionServiceFieldInfo.SetValue(autoCompletion, autoCompletionService);
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
            var allowAutoCompletion = AllowAutoCompletion;
            var filterSource = FilterSource;

            SetCurrentValue(AllowAutoCompletionProperty, false);
            SetCurrentValue(FilterSourceProperty, null);

            try
            {
                SetCurrentValue(TextProperty, string.Empty);
            }
            finally
            {
                SetCurrentValue(FilterSourceProperty, filterSource);
                SetCurrentValue(AllowAutoCompletionProperty, allowAutoCompletion);
            }
        }

        private bool CanClearFilter()
        {
            return !string.IsNullOrWhiteSpace(Text);
        }

        private void OnTextChanged()
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
