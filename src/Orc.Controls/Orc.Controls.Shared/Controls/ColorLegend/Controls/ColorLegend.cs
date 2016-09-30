// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorLegend.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Windows;

    /// <summary>
    /// Control to show color legend with checkboxes for each color.
    /// </summary>
    [TemplatePart(Name = "PART_List", Type = typeof (ListBox))]
    [TemplatePart(Name = "PART_Popup_Color_Board", Type = typeof (Popup))]
    [TemplatePart(Name = "PART_UnselectAll", Type = typeof (ButtonBase))]
    [TemplatePart(Name = "PART_All_Visible", Type = typeof (CheckBox))]
    [TemplatePart(Name = "PART_Settings_Button", Type = typeof (DropDownButton))]
    public class ColorLegend : HeaderedContentControl
    {
        #region Events
        /// <summary>
        /// Selection changed event.
        /// </summary>
        public event EventHandler<EventArgs> SelectionChanged;
        #endregion

        #region Fields
        private ColorBoard _colorBoard;

        private ListBox _listBox;

        private Popup _popup;

        private ButtonBase _button;

        private CheckBox _checkBox;

        private IColorLegendItem _currentColorLegendItem;

        private bool _manualBindingReady;

        private bool _isUpdatingAllVisible;

        private ChangeNotificationWrapper _changeNotificationWrapper;

        private Color _previousColor;
        #endregion

        #region Constructors
        static ColorLegend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorLegend), new FrameworkPropertyMetadata(typeof (ColorLegend)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorLegend" /> class.
        /// </summary>
        public ColorLegend()
        {
            ChangeColor = new Command<object>(OnChangeColorExecute, OnChangeColorCanExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the operation color attribute.
        /// </summary>
        [ObsoleteEx(Message = "No longer required, legend now respects the Header property", TreatAsErrorFromVersion = "1.3", RemoveInVersion = "2.0")]
        public string OperationColorAttribute
        {
            get { return (string) GetValue(OperationColorAttributeProperty); }
            set { SetValue(OperationColorAttributeProperty, value); }
        }

        /// <summary>
        /// The operation color attribute property.
        /// </summary>
        public static readonly DependencyProperty OperationColorAttributeProperty = DependencyProperty.Register("OperationColorAttribute",
            typeof (string), typeof (ColorLegend), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets a value indicating whether color can be edited or not.
        /// </summary>
        public bool AllowColorEditing
        {
            get { return (bool) GetValue(AllowColorEditingProperty); }
            set { SetValue(AllowColorEditingProperty, value); }
        }

        /// <summary>
        /// Property indicating whether color can be edited or not
        /// </summary>
        public static readonly DependencyProperty AllowColorEditingProperty = DependencyProperty.Register("AllowColorEditing",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true, (sender, e) => ((ColorLegend) sender).OnAllowColorEditingChanged()));

        /// <summary>
        /// Gets or sets a value indicating whether search box is shown or not.
        /// </summary>
        public bool ShowSearchBox
        {
            get { return (bool) GetValue(ShowSearchBoxProperty); }
            set { SetValue(ShowSearchBoxProperty, value); }
        }

        /// <summary>
        /// Property indicating whether search box is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowSearchBoxProperty = DependencyProperty.Register("ShowSearchBox",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether tool box is shown or not.
        /// </summary>
        public bool ShowToolBox
        {
            get { return (bool) GetValue(ShowToolBoxProperty); }
            set { SetValue(ShowToolBoxProperty, value); }
        }

        /// <summary>
        /// Property indicating whethertop toolbox is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowToolBoxProperty = DependencyProperty.Register("ShowToolBox",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether tool box is shown or not.
        /// </summary>
        public bool ShowBottomToolBox
        {
            get { return (bool) GetValue(ShowBottomToolBoxProperty); }
            set { SetValue(ShowBottomToolBoxProperty, value); }
        }

        /// <summary>
        /// Property indicating whether bottom tool box is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowBottomToolBoxProperty = DependencyProperty.Register("ShowBottomToolBox",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether settings button is shown or not.
        /// </summary>
        [ObsoleteEx(Message= "No longer required due to consistency, use ShowSettingsBox instead", ReplacementTypeOrMember = "ShowSettingsBox", TreatAsErrorFromVersion = "1.3", RemoveInVersion = "2.0")]
        public bool ShowSettings
        {
            get { return (bool)GetValue(ShowSettingsProperty); }
            set { SetValue(ShowSettingsProperty, value); }
        }

        /// <summary>
        /// Property indicating whether search box is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowSettingsProperty = DependencyProperty.Register("ShowSettings",
            typeof(bool), typeof(ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether settings button is shown or not.
        /// </summary>
        public bool ShowSettingsBox
        {
            get { return (bool) GetValue(ShowSettingsBoxProperty); }
            set { SetValue(ShowSettingsBoxProperty, value); }
        }

        /// <summary>
        /// Property indicating whether search box is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowSettingsBoxProperty = DependencyProperty.Register("ShowSettingsBox",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether settings button is shown or not.
        /// </summary>
        public bool ShowColorVisibilityControls
        {
            get { return (bool) GetValue(ShowColorVisibilityControlsProperty); }
            set { SetValue(ShowColorVisibilityControlsProperty, value); }
        }

        /// <summary>
        /// Property indicating whether search box is shown or not.
        /// </summary>
        public static readonly DependencyProperty ShowColorVisibilityControlsProperty = DependencyProperty.Register("ShowColorVisibilityControls",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true, (sender, e) => ((ColorLegend) sender).OnShowColorVisibilityControlsChanged()));

        /// <summary>
        /// Gets or sets a value indicating whether user editing current color.
        /// </summary>
        public bool IsColorSelecting
        {
            get { return (bool) GetValue(IsColorSelectingProperty); }
            set { SetValue(IsColorSelectingProperty, value); }
        }

        /// <summary>
        /// The is drop down open property.
        /// </summary>
        public static readonly DependencyProperty IsColorSelectingProperty = DependencyProperty.Register("IsColorSelecting",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(false, (sender, e) => ((ColorLegend) sender).OnIsColorSelectingPropertyChanged()));

        /// <summary>
        /// Gets or sets a value indicating whether regex is used when search is performed.
        /// </summary>
        public bool UseRegexFiltering
        {
            get { return (bool) GetValue(UseRegexFilteringProperty); }
            set { SetValue(UseRegexFilteringProperty, value); }
        }

        /// <summary>
        /// Property indicating whether search is performing using regex or not.
        /// </summary>
        public static readonly DependencyProperty UseRegexFilteringProperty = DependencyProperty.Register("UseRegexFiltering",
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether user editing current color.
        /// </summary>
        public Color EditingColor
        {
            get { return (Color) GetValue(EditingColorProperty); }
            set { SetValue(EditingColorProperty, value); }
        }

        /// <summary>
        /// The current color property.
        /// </summary>
        public static readonly DependencyProperty EditingColorProperty = DependencyProperty.Register("EditingColor", typeof (Color),
            typeof (ColorLegend), new PropertyMetadata(Colors.White, (sender, e) => ((ColorLegend) sender).OnEditingColorChanged()));

        /// <summary>
        /// Gets or sets filter for list of color.
        /// </summary>
        public string Filter
        {
            get { return (string) GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        /// <summary>
        /// Expose filter property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter",
            typeof (string), typeof (ColorLegend), new PropertyMetadata(null, (sender, e) => ((ColorLegend) sender).OnFilterChanged()));

        /// <summary>
        /// Gets or sets source for color items.
        /// </summary>
        public IEnumerable<IColorLegendItem> ItemsSource
        {
            get { return (IEnumerable<IColorLegendItem>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Property for colors list.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof (IEnumerable<IColorLegendItem>), typeof (ColorLegend), new FrameworkPropertyMetadata(null,
                (sender, e) => ((ColorLegend) sender).OnItemsSourceChanged(e.OldValue as IEnumerable<IColorLegendItem>, e.NewValue as IEnumerable<IColorLegendItem>)));

        /// <summary>
        /// Gets or sets a value indicating whether is all visible.
        /// </summary>
        public bool? IsAllVisible
        {
            get { return (bool?) GetValue(IsAllVisibleProperty); }
            set { SetValue(IsAllVisibleProperty, value); }
        }

        /// <summary>
        /// The is all visible property.
        /// </summary>
        public static readonly DependencyProperty IsAllVisibleProperty = DependencyProperty.Register("IsAllVisible",
            typeof (bool?), typeof (ColorLegend), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((ColorLegend) sender).OnIsAllVisibleChanged()));

        /// <summary>
        /// Gets or sets a source for color items respecting current filter value.
        /// </summary>
        public IEnumerable<IColorLegendItem> FilteredItemsSource
        {
            get { return (IEnumerable<IColorLegendItem>) GetValue(FilteredItemsSourceProperty); }
            set { SetValue(FilteredItemsSourceProperty, value); }
        }

        /// <summary>
        /// Property for colors list.
        /// </summary>
        public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register("FilteredItemsSource",
            typeof (IEnumerable<IColorLegendItem>), typeof (ColorLegend), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the filtered items ids.
        /// </summary>
        public IEnumerable<string> FilteredItemsIds
        {
            get { return (IEnumerable<string>) GetValue(FilteredItemsIdsProperty); }
            set { SetValue(FilteredItemsIdsProperty, value); }
        }

        /// <summary>
        /// Property to store all visible now ids.
        /// </summary>
        public static readonly DependencyProperty FilteredItemsIdsProperty = DependencyProperty.Register("FilteredItemsIds",
            typeof (IEnumerable<string>), typeof (ColorLegend), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets filter watermark string we use in search textbox.
        /// </summary>
        public string FilterWatermark
        {
            get { return (string) GetValue(FilterWatermarkProperty); }
            set { SetValue(FilterWatermarkProperty, value); }
        }

        /// <summary>
        /// Property for filter watermark.
        /// </summary>
        public static readonly DependencyProperty FilterWatermarkProperty = DependencyProperty.Register("FilterWatermark",
            typeof (string), typeof (ColorLegend), new PropertyMetadata("Search"));

        /// <summary>
        /// Gets or sets list of selected items.
        /// </summary>
        public IEnumerable<IColorLegendItem> SelectedColorItems
        {
            get { return (IEnumerable<IColorLegendItem>) GetValue(SelectedColorItemsProperty); }
            set { SetValue(SelectedColorItemsProperty, value); }
        }

        /// <summary>
        /// The selected items property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorItemsProperty = DependencyProperty.RegisterAttached("SelectedColorItems",
            typeof (IEnumerable<IColorLegendItem>), typeof (ColorLegend), new PropertyMetadata(null, (sender, e) => ((ColorLegend) sender).OnSelectedColorItemsChanged()));

        public Brush AccentColorBrush
        {
            get { return (Brush) GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof (Brush),
            typeof (ColorLegend), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((ColorLegend) sender).OnAccentColorBrushChanged()));
        #endregion

        #region Commands
        public Command<object> ChangeColor { get; private set; }

        private bool OnChangeColorCanExecute(object parameter)
        {
            var values = (object[]) parameter;
            var colorProvider = (IColorLegendItem) values[1];

            if (colorProvider == null)
            {
                return false;
            }

            if (!AllowColorEditing)
            {
                return false;
            }

            return true;
        }

        private void OnChangeColorExecute(object parameter)
        {
            var parameterValues = (object[]) parameter;

            var currentButton = parameterValues[0];
            var colorLegendItem = (IColorLegendItem) parameterValues[1];

            _currentColorLegendItem = colorLegendItem;
            _popup.PlacementTarget = (Button) currentButton;

            _previousColor = EditingColor = colorLegendItem.Color;
            IsColorSelecting = true;
        }
        #endregion

        #region Methods
        private void OnItemsSourceChanged(IEnumerable<IColorLegendItem> oldValue, IEnumerable<IColorLegendItem> newValue)
        {
            if (_changeNotificationWrapper != null)
            {
                _changeNotificationWrapper.CollectionItemPropertyChanged -= OnColorProviderPropertyChanged;
                _changeNotificationWrapper.UnsubscribeFromAllEvents();
                _changeNotificationWrapper = null;
            }

            FilteredItemsSource = GetFilteredItems();
            FilteredItemsIds = FilteredItemsSource == null ? null : FilteredItemsSource.Select(cp => cp.Id);

            if (newValue != null)
            {
                _changeNotificationWrapper = new ChangeNotificationWrapper(newValue);
                _changeNotificationWrapper.CollectionItemPropertyChanged += OnColorProviderPropertyChanged;
            }

            UpdateIsAllVisible();
        }

        private void OnColorProviderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.HasPropertyChanged("IsChecked"))
            {
                UpdateIsAllVisible();
            }
        }

        private void UpdateIsAllVisible()
        {
            _isUpdatingAllVisible = true;

            var allChecked = true;
            var allUnchecked = true;

            var filteredItems = GetFilteredItems();
            if (filteredItems != null)
            {
                foreach (var filteredItem in filteredItems)
                {
                    if (filteredItem.IsChecked)
                    {
                        allUnchecked = false;
                    }
                    else
                    {
                        allChecked = false;
                    }
                }
            }

            IsAllVisible = (allChecked ? true : allUnchecked ? false : (bool?) null);

            _isUpdatingAllVisible = false;
        }

        private void OnIsAllVisibleChanged()
        {
            if (_isUpdatingAllVisible)
            {
                return;
            }

            var isAllVisible = IsAllVisible;
            if (!isAllVisible.HasValue)
            {
                // Don't allow a user to manually pick null, false is next in "line" (it's true => null => false)
                IsAllVisible = false;
                return;
            }

            var filteredItems = GetFilteredItems();
            if (filteredItems != null)
            {
                foreach (var filteredItem in filteredItems)
                {
                    filteredItem.IsChecked = isAllVisible.Value;
                }
            }
        }

        private void OnSelectedColorItemsChanged()
        {
            SetSelectedList(SelectedColorItems);
        }

        private void OnShowColorVisibilityControlsChanged()
        {
            UpdateVisibilityControlsVisibility();
        }

        private void OnAllowColorEditingChanged()
        {
            UpdateColorEditingControlsVisibility();
        }

        private void OnEditingColorChanged()
        {
            var currentColorLegendItem = _currentColorLegendItem;
            if (currentColorLegendItem != null)
            {
                currentColorLegendItem.Color = EditingColor;
            }
        }

        private void OnFilterChanged()
        {
            FilteredItemsSource = GetFilteredItems();
            FilteredItemsIds = FilteredItemsSource == null ? null : FilteredItemsSource.Select(cp => cp.Id);
        }

        private void OnIsColorSelectingPropertyChanged()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;

            _listBox = (ListBox) GetTemplateChild("PART_List");
            _popup = (Popup) GetTemplateChild("PART_Popup_Color_Board");
            _button = (ButtonBase) GetTemplateChild("PART_UnselectAll");
            _checkBox = (CheckBox) GetTemplateChild("PART_All_Visible");

            var settingsButton = (DropDownButton) GetTemplateChild("PART_Settings_Button");
            if (settingsButton != null)
            {
                settingsButton.ContentLayoutUpdated += (sender, args) =>
                {
                    if (_manualBindingReady)
                    {
                        return;
                    }

                    var settingsContent = sender as ContentControl;
                    if(settingsContent != null)
                    {
                        var contentPresenter = settingsContent.FindVisualDescendantByType<ContentPresenter>();
                        if (contentPresenter == null)
                        {
                            return;
                        }

                        var content = contentPresenter.Content;
                        if (content == null)
                        {
                            return;
                        }

                        var dependencyObject = content as DependencyObject;
                        if (dependencyObject != null)
                        {
                            var qryAllChecks = dependencyObject.Descendents().OfType<CheckBox>();

                            var allChecks = qryAllChecks as CheckBox[] ?? qryAllChecks.ToArray();
                            var cbVisibilitySetting = allChecks.First(cb => cb.Name == "cbVisibilitySetting");
                            if (cbVisibilitySetting != null)
                            {
                                cbVisibilitySetting.IsChecked = ShowColorVisibilityControls;
                                cbVisibilitySetting.Checked += (o, e) => ShowColorVisibilityControls = true;
                                cbVisibilitySetting.Unchecked += (o, e) => ShowColorVisibilityControls = false;
                            }

                            var cbColorEditingSetting = allChecks.First(cb => cb.Name == "cbColorEditingSetting");
                            if (cbColorEditingSetting != null)
                            {
                                cbColorEditingSetting.IsChecked = AllowColorEditing;
                                cbColorEditingSetting.Checked += (o, e) => AllowColorEditing = true;
                                cbColorEditingSetting.Unchecked += (o, e) => AllowColorEditing = false;
                            }

                            var cbUseRegexSetting = allChecks.First(cb => cb.Name == "cbUseRegexSetting");
                            if (cbUseRegexSetting != null)
                            {
                                cbUseRegexSetting.IsChecked = UseRegexFiltering;
                                cbUseRegexSetting.Checked += (o, e) => UseRegexFiltering = true;
                                cbUseRegexSetting.Unchecked += (o, e) => UseRegexFiltering = false;
                            }
                        }

                        _manualBindingReady = true;
                    }
                };
            }

            if (_listBox != null)
            {
                _listBox.SelectionChanged += (sender, args) =>
                {
                    SelectedColorItems = GetSelectedList();
                    var handler = SelectionChanged;
                    if (handler != null)
                    {
                        handler(sender, args);
                    }
                };

                _listBox.SelectionMode = SelectionMode.Extended;

                _listBox.LayoutUpdated += (sender, args) =>
                {
                    UpdateVisibilityControlsVisibility();
                    UpdateColorEditingControlsVisibility();
                };
            }

            if (_button != null)
            {
                _button.Click += (s, e) =>
                {
                    if (_listBox != null)
                    {
                        _listBox.SelectedIndex = -1;
                    }
                };
            }

            if (_checkBox != null)
            {
                _checkBox.Checked += (sender, args) => IsAllVisible = true;
                _checkBox.Unchecked += (sender, args) => IsAllVisible = false;
            }

            _colorBoard = new ColorBoard();
            if (_popup != null)
            {
                _popup.Child = _colorBoard;
            }

            var b = new Binding("Color")
            {
                Mode = BindingMode.TwoWay,
                Source = _colorBoard
            };

            SetBinding(EditingColorProperty, b);

            _colorBoard.DoneClicked += ColorBoardDoneClicked;
            _colorBoard.CancelClicked += ColorBoardCancelClicked;
        }

        public void UpdateVisibilityControlsVisibility()
        {
            if (_listBox == null)
            {
                return;
            }

            var qryAllChecks = _listBox.Descendents().OfType<CheckBox>();

            foreach (CheckBox check in qryAllChecks)
            {
                check.Visibility = ShowColorVisibilityControls ? Visibility.Visible : Visibility.Collapsed;
            }

            _checkBox.Visibility = ShowColorVisibilityControls ? Visibility.Visible : Visibility.Collapsed;
        }

        public void UpdateColorEditingControlsVisibility()
        {
            if (_listBox == null)
            {
                return;
            }

            var qryAllArrows = _listBox.Descendents().OfType<System.Windows.Shapes.Path>().Where(p => p.Name == "arrow");
            foreach (System.Windows.Shapes.Path path in qryAllArrows)
            {
                path.Visibility = AllowColorEditing ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public IEnumerable<IColorLegendItem> GetSelectedList()
        {
            return from object item in _listBox.SelectedItems select item as IColorLegendItem;
        }

        private bool _settingSelectedList;

        public void SetSelectedList(IEnumerable<IColorLegendItem> selectedList)
        {
            if (_listBox == null || selectedList == null || _settingSelectedList)
            {
                return;
            }

            _settingSelectedList = true;

            try
            {
                var colorLegendItems = selectedList as IList<IColorLegendItem> ?? selectedList.ToList();

                _listBox.SelectedItems.Clear();
                var itemsSource = ItemsSource;
                if (itemsSource == null)
                {
                    return;
                }

                foreach (var legendItem in itemsSource)
                {
                    legendItem.IsSelected = false;
                }

                foreach (var legendItem in colorLegendItems)
                {
                    legendItem.IsSelected = true;
                    _listBox.SelectedItems.Add(legendItem);
                }
            }
            finally
            {
                _settingSelectedList = false;
            }
        }

        private string ConstructWildcardRegex(string pattern)
        {
            // Always add a wildcard at the end of the pattern
            pattern = "*" + pattern.Trim('*') + "*";

            // Make it case insensitive by default
            return "(?i)^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
        }

        protected IEnumerable<IColorLegendItem> GetFilteredItems()
        {
            var items = (IEnumerable<IColorLegendItem>) GetValue(ItemsSourceProperty);
            var filter = (string) GetValue(FilterProperty);

            if ((items == null) || string.IsNullOrEmpty(filter))
            {
                return items;
            }

            try
            {
                Regex regex = UseRegexFiltering ? new Regex(filter) : new Regex(ConstructWildcardRegex(filter));
                return items.Where(cp => regex.IsMatch(cp.Description));
            }
            catch (Exception)
            {
                return items;
            }
        }

        private void ColorBoardDoneClicked(object sender, RoutedEventArgs e)
        {
            EditingColor = _colorBoard.Color;
            _popup.IsOpen = false;
        }

        private void ColorBoardCancelClicked(object sender, RoutedEventArgs e)
        {
            _colorBoard.Color = _previousColor;
            _popup.IsOpen = false;
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("ColorLegend");
            }
        }
        #endregion
    }
}