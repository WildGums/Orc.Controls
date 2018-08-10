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
    using Catel;
    using Catel.Data;
    using Catel.MVVM;

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
        private bool _isUpdatingAllVisible;
        private ChangeNotificationWrapper _changeNotificationWrapper;
        private Color _previousColor;
        private bool _handlingListBoxSelectionChanged;
        private bool _settingSelectedList;
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
        public static readonly DependencyProperty AllowColorEditingProperty = DependencyProperty.Register(nameof(AllowColorEditing),
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
        public static readonly DependencyProperty ShowSearchBoxProperty = DependencyProperty.Register(nameof(ShowSearchBox),
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
        public static readonly DependencyProperty ShowToolBoxProperty = DependencyProperty.Register(nameof(ShowToolBox),
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
        public static readonly DependencyProperty ShowBottomToolBoxProperty = DependencyProperty.Register(nameof(ShowBottomToolBox),
            typeof (bool), typeof (ColorLegend), new PropertyMetadata(true));


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
        public static readonly DependencyProperty ShowSettingsBoxProperty = DependencyProperty.Register(nameof(ShowSettingsBox),
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
        public static readonly DependencyProperty ShowColorVisibilityControlsProperty = DependencyProperty.Register(nameof(ShowColorVisibilityControls),
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
        public static readonly DependencyProperty IsColorSelectingProperty = DependencyProperty.Register(nameof(IsColorSelecting),
            typeof(bool), typeof(ColorLegend), new PropertyMetadata(false));


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
        public static readonly DependencyProperty EditingColorProperty = DependencyProperty.Register(nameof(EditingColor), typeof (Color),
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
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter),
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
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
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
        public static readonly DependencyProperty IsAllVisibleProperty = DependencyProperty.Register(nameof(IsAllVisible),
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
        public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register(nameof(FilteredItemsSource),
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
        public static readonly DependencyProperty FilteredItemsIdsProperty = DependencyProperty.Register(nameof(FilteredItemsIds),
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
        public static readonly DependencyProperty FilterWatermarkProperty = DependencyProperty.Register(nameof(FilterWatermark),
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
        public static readonly DependencyProperty SelectedColorItemsProperty = DependencyProperty.RegisterAttached(nameof(SelectedColorItems),
            typeof (IEnumerable<IColorLegendItem>), typeof (ColorLegend), new PropertyMetadata(null, (sender, e) => ((ColorLegend) sender).OnSelectedColorItemsChanged()));

        public Brush AccentColorBrush
        {
            get { return (Brush) GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(nameof(AccentColorBrush), typeof (Brush),
            typeof (ColorLegend), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((ColorLegend) sender).OnAccentColorBrushChanged()));
        #endregion

        #region Commands
        public Command<object> ChangeColor { get; private set; }

        private bool OnChangeColorCanExecute(object parameter)
        {
            var values = (object[]) parameter;
            var colorProvider = (IColorLegendItem) values[1];

            return colorProvider != null && AllowColorEditing;
        }

        private void OnChangeColorExecute(object parameter)
        {
            var parameterValues = (object[]) parameter;

            var currentButton = parameterValues[0];
            var colorLegendItem = (IColorLegendItem) parameterValues[1];

            _currentColorLegendItem = colorLegendItem;
            _popup.SetCurrentValue(Popup.PlacementTargetProperty, (Button) currentButton);

            var color = colorLegendItem.Color;
            SetCurrentValue(EditingColorProperty, color);
            _previousColor = color;
            SetCurrentValue(IsColorSelectingProperty, true);
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

            SetCurrentValue(FilteredItemsSourceProperty, GetFilteredItems());
            SetCurrentValue(FilteredItemsIdsProperty, FilteredItemsSource?.Select(cp => cp.Id));

            if (newValue != null)
            {
                _changeNotificationWrapper = new ChangeNotificationWrapper(newValue);
                _changeNotificationWrapper.CollectionItemPropertyChanged += OnColorProviderPropertyChanged;
            }

            UpdateIsAllVisible();
        }

        private void OnColorProviderPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
#pragma warning disable WPF1014
            if (e.HasPropertyChanged(nameof(IColorLegendItem.IsChecked)))
#pragma warning restore WPF1014
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
                    if (!filteredItem.IsChecked)
                    {
                        allChecked = false; 
                    }
                    else
                    {
                        allUnchecked = false;
                    }
                }
            }

            bool? isAllVisible = null;

            if (allChecked)
            {
                isAllVisible = true;
            }
            else if (allUnchecked)
            {
                isAllVisible = false;
            }

            SetCurrentValue(IsAllVisibleProperty, isAllVisible);

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
                SetCurrentValue(IsAllVisibleProperty, false);
                return;
            }

            var filteredItems = GetFilteredItems();
            if (filteredItems == null)
            {
                return;
            }

            foreach (var filteredItem in filteredItems)
            {
                filteredItem.IsChecked = isAllVisible.Value;
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
            SetCurrentValue(FilteredItemsSourceProperty, GetFilteredItems());
            SetCurrentValue(FilteredItemsIdsProperty, FilteredItemsSource?.Select(cp => cp.Id));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);

            _listBox = (ListBox) GetTemplateChild("PART_List");
            _popup = (Popup) GetTemplateChild("PART_Popup_Color_Board");
            _button = (ButtonBase) GetTemplateChild("PART_UnselectAll");
            _checkBox = (CheckBox) GetTemplateChild("PART_All_Visible");

            if (_listBox != null)
            {
                _listBox.SelectionChanged += OnListBoxSelectionChanged;

                _listBox.SetCurrentValue(ListBox.SelectionModeProperty, SelectionMode.Extended);

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
                    _listBox?.SetCurrentValue(Selector.SelectedIndexProperty, -1);
                };
            }

            if (_checkBox != null)
            {
                _checkBox.Checked += (sender, args) => SetCurrentValue(IsAllVisibleProperty, true);
                _checkBox.Unchecked += (sender, args) => SetCurrentValue(IsAllVisibleProperty, false);
            }

            _colorBoard = new ColorBoard();

            _popup?.SetCurrentValue(Popup.ChildProperty, _colorBoard);

            var b = new Binding(nameof(ColorBoard.Color))
            {
                Mode = BindingMode.TwoWay,
                Source = _colorBoard
            };

            SetBinding(EditingColorProperty, b);

            _colorBoard.DoneClicked += ColorBoardDoneClicked;
            _colorBoard.CancelClicked += ColorBoardCancelClicked;
        }

        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_handlingListBoxSelectionChanged)
            {
                return;
            }

            _handlingListBoxSelectionChanged = true;

            try
            {
                SetCurrentValue(SelectedColorItemsProperty, GetSelectedList());
                SelectionChanged.SafeInvoke(sender, e);
            }
            finally
            {
                _handlingListBoxSelectionChanged = false;
            }
        }

        public void UpdateVisibilityControlsVisibility()
        {
            if (_listBox == null)
            {
                return;
            }
            
            var qryAllChecks = _listBox.GetDescendents().OfType<CheckBox>();

            foreach (var check in qryAllChecks)
            {
                check.SetCurrentValue(VisibilityProperty, ShowColorVisibilityControls ? Visibility.Visible : Visibility.Collapsed);
            }

            _checkBox.SetCurrentValue(VisibilityProperty, ShowColorVisibilityControls ? Visibility.Visible : Visibility.Collapsed);
        }

        public void UpdateColorEditingControlsVisibility()
        {
            if (_listBox == null)
            {
                return;
            }

            var qryAllArrows = _listBox.GetDescendents().OfType<System.Windows.Shapes.Path>().Where(p => p.Name == "arrow");
            foreach (var path in qryAllArrows)
            {
                path.SetCurrentValue(VisibilityProperty, AllowColorEditing ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public IEnumerable<IColorLegendItem> GetSelectedList()
        {
            return from object item in _listBox.SelectedItems select item as IColorLegendItem;
        }        

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

                if (!_handlingListBoxSelectionChanged)
                {
                    _listBox.SelectedItems.Clear();
                }

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

                    if (!_handlingListBoxSelectionChanged)
                    {
                        _listBox.SelectedItems.Add(legendItem);
                    }
                }
            }
            finally
            {
                _settingSelectedList = false;
            }
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
                var regex = new Regex(filter.GetRegexStringFromSearchPattern());
                return items.Where(cp => regex.IsMatch(cp.Description));
            }
            catch (Exception)
            {
                return items;
            }
        }

        private void ColorBoardDoneClicked(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(EditingColorProperty, _colorBoard.Color);
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }

        private void ColorBoardCancelClicked(object sender, RoutedEventArgs e)
        {
            _colorBoard.SetCurrentValue(ColorBoard.ColorProperty, _previousColor);
            _popup.SetCurrentValue(Popup.IsOpenProperty, false);
        }

        private void OnAccentColorBrushChanged()
        {
            if (!(AccentColorBrush is SolidColorBrush solidColorBrush))
            {
                return;
            }

            var accentColor = solidColorBrush.Color;
            accentColor.CreateAccentColorResourceDictionary(nameof(ColorLegend));
        }
        #endregion
    }
}
