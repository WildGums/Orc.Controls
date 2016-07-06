// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expander.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public enum ExpandDirection
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3,
    }

    public class Expander : HeaderedContentControl
    {
        #region Fields
        private GridLength? _expandDistance;
        private GridLength _collapsedDistance = GridLength.Auto;
        #endregion

        #region Constructors
        public Expander()
        {
            DefaultStyleKey = typeof (Expander);
        }
        #endregion

        #region Properties
        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof (bool), typeof (Expander), new PropertyMetadata(false, OnIsExpandedPropertyChanged));

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection) GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register("ExpandDirection", typeof (ExpandDirection), typeof (Expander), new PropertyMetadata(ExpandDirection.Left));

        public bool AutoResizeGrid
        {
            get { return (bool) GetValue(AutoResizeGridProperty); }
            set { SetValue(AutoResizeGridProperty, value); }
        }

        public static readonly DependencyProperty AutoResizeGridProperty = DependencyProperty.Register("AutoResizeGrid", typeof (bool), typeof (Expander), new PropertyMetadata(false));

        public Brush AccentColorBrush
        {
            get { return (Brush) GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof (Brush), typeof (Expander), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((Expander) sender).OnAccentColorBrushChanged()));
        #endregion

        #region Methods
        private static void OnIsExpandedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var expander = d as Expander;
            if ((bool) e.NewValue)
            {
                expander.OnExpanded();
            }
            else
            {
                expander.OnCollapsed();
            }
        }

        protected virtual void OnCollapsed()
        {
            if (!AutoResizeGrid)
            {
                return;
            }

            if (!(Parent is Grid))
            {
                return;
            }

            var grid = (Grid) Parent;
            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                {
                    var column = Grid.GetColumn(this);
                    _expandDistance = grid.ColumnDefinitions[column].Width;
                    grid.ColumnDefinitions[column].Width = _collapsedDistance;
                    break;
                }
                case ExpandDirection.Right:
                {
                    var column = Grid.GetColumn(this);
                    _expandDistance = grid.ColumnDefinitions[column].Width;
                    grid.ColumnDefinitions[column].Width = _collapsedDistance;
                    break;
                }
                case ExpandDirection.Up:
                {
                    var row = Grid.GetRow(this);
                    _expandDistance = grid.RowDefinitions[row].Height;
                    grid.RowDefinitions[row].Height = _collapsedDistance;
                    break;
                }
                case ExpandDirection.Down:
                {
                    var row = Grid.GetRow(this);
                    _expandDistance = grid.RowDefinitions[row].Height;
                    grid.RowDefinitions[row].Height = _collapsedDistance;
                    break;
                }
            }
        }

        protected virtual void OnExpanded()
        {
            if (!AutoResizeGrid)
            {
                return;
            }

            if (!(Parent is Grid))
            {
                return;
            }

            var grid = (Grid) Parent;
            var header = Header as FrameworkElement;

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                {
                    _collapsedDistance = HasHeader ? new GridLength(header?.ActualWidth ?? 0, GridUnitType.Pixel) : GridLength.Auto;
                    if (_expandDistance.HasValue)
                    {
                        var column = Grid.GetColumn(this);
                        grid.ColumnDefinitions[column].Width = _expandDistance.Value;
                    }
                    break;
                }
                case ExpandDirection.Right:
                {
                    _collapsedDistance = HasHeader ? new GridLength(header?.ActualWidth ?? 0, GridUnitType.Pixel) : GridLength.Auto;
                    if (_expandDistance.HasValue)
                    {
                        var column = Grid.GetColumn(this);
                        grid.ColumnDefinitions[column].Width = _expandDistance.Value;
                    }
                    break;
                }
                case ExpandDirection.Up:
                {
                    _collapsedDistance = HasHeader ? new GridLength(header?.ActualHeight ?? 0, GridUnitType.Pixel) : GridLength.Auto;
                    if (_expandDistance.HasValue)
                    {
                        var row = Grid.GetRow(this);
                        grid.RowDefinitions[row].Height = _expandDistance.Value;
                    }
                    break;
                }
                case ExpandDirection.Down:
                {
                    _collapsedDistance = HasHeader ? new GridLength(header?.ActualHeight ?? 0, GridUnitType.Pixel) : GridLength.Auto;
                    if (_expandDistance.HasValue)
                    {
                        var row = Grid.GetRow(this);
                        grid.RowDefinitions[row].Height = _expandDistance.Value;
                    }
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("Expander");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
        }
        #endregion
    }
}