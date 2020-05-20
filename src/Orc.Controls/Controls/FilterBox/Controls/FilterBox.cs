// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Windows.Interactivity;
    using Theming;

   // [TemplatePart(Name = "PART_FilterTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class FilterBox : TextBox
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Command _clearFilter;
        private Button _clearButton;
        private AutoCompletion _autoCompletion;
        #endregion

        #region Constructors
        public FilterBox()
        {
            _clearFilter = new Command(OnClearFilter, CanClearFilter);
        }
        #endregion

        #region Dependency Properties
        public bool AllowAutoCompletion
        {
            get { return (bool)GetValue(AllowAutoCompletionProperty); }
            set { SetValue(AllowAutoCompletionProperty, value); }
        }

        public static readonly DependencyProperty AllowAutoCompletionProperty = DependencyProperty.Register(
            nameof(AllowAutoCompletion), typeof(bool), typeof(FilterBox), 
            new PropertyMetadata(true, (sender, args) => ((FilterBox)sender).OnAllowAutoCompletionChanged()));

        public IEnumerable FilterSource
        {
            get { return (IEnumerable)GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register(nameof(FilterSource), typeof(IEnumerable),
            typeof(FilterBox), new FrameworkPropertyMetadata(null,
                (sender, args) => ((FilterBox)sender).OnFilterSourceChanged()));

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string),
            typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                , (sender, args) => ((FilterBox)sender).OnPropertyNameChanged()));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string),
            typeof(FilterBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clearButton = GetTemplateChild("PART_ClearButton") as Button;
            if (_clearButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClearButton'");
            }
            _clearButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _clearFilter);
            _clearButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, null);

            this.AttachBehavior<UpdateBindingOnTextChanged>();
            this.AttachBehavior<SelectTextOnFocus>();

            _autoCompletion = this.AttachBehavior<AutoCompletion>();
            UpdateAutoCompletion();

            var serviceEventArg = new InitializingAutoCompletionServiceEventArgs();
            OnInitializingAutoCompletionService(serviceEventArg);
            var autoCompletionService = serviceEventArg.AutoCompletionService;
            if (autoCompletionService == null)
            {
                return;
            }
            
            //Hack
            var autoCompletionServiceFieldInfo = _autoCompletion.GetType().GetField("_autoCompletionService", BindingFlags.Instance | BindingFlags.NonPublic);
            if (autoCompletionServiceFieldInfo != null)
            {
                autoCompletionServiceFieldInfo.SetValue(_autoCompletion, autoCompletionService);
            }
        }

        private void UpdateAutoCompletion()
        {
            if (_autoCompletion is null)
            {
                return;
            }

            _autoCompletion.SetCurrentValue(AutoCompletion.PropertyNameProperty, PropertyName);
            _autoCompletion.SetCurrentValue(AutoCompletion.ItemsSourceProperty, FilterSource);
            _autoCompletion.SetCurrentValue(AutoCompletion.UseAutoCompletionServiceProperty, AllowAutoCompletion);
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

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            _clearFilter.RaiseCanExecuteChanged();

            base.OnTextChanged(e);
        }

        protected virtual void OnInitializingAutoCompletionService(InitializingAutoCompletionServiceEventArgs e)
        {
            InitializingAutoCompletionService?.Invoke(this, e);
        }

        private void OnAllowAutoCompletionChanged()
        {
            UpdateAutoCompletion();
        }

        private void OnFilterSourceChanged()
        {
            UpdateAutoCompletion();
        }
        private void OnPropertyNameChanged()
        {
            UpdateAutoCompletion();
        }
        #endregion

        public event EventHandler<InitializingAutoCompletionServiceEventArgs> InitializingAutoCompletionService;
    }
}
