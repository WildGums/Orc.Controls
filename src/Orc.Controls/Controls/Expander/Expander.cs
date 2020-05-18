// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expander.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class Expander : HeaderedContentControl
    {
        #region Fields
        private GridLength? _expandDistance;
        #endregion

        #region Constructors
        public Expander()
        {
            DefaultStyleKey = typeof(Expander);
        }
        #endregion

        #region Properties
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded),
            typeof(bool), typeof(Expander), new PropertyMetadata(false, OnIsExpandedChanged));

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(nameof(ExpandDirection),
            typeof(ExpandDirection), typeof(Expander), new PropertyMetadata(ExpandDirection.Left));

        public bool AutoResizeGrid
        {
            get { return (bool)GetValue(AutoResizeGridProperty); }
            set { SetValue(AutoResizeGridProperty, value); }
        }

        public static readonly DependencyProperty AutoResizeGridProperty = DependencyProperty.Register(nameof(AutoResizeGrid),
            typeof(bool), typeof(Expander), new PropertyMetadata(false));
        #endregion

        #region Methods
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Expander expander))
            {
                return;
            }

            if ((bool)e.NewValue)
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

            if (!(Parent is Grid grid))
            {
                return;
            }

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                    {
                        var column = Grid.GetColumn(this);
                        _expandDistance = grid.ColumnDefinitions[column].Width;
                        grid.ColumnDefinitions[column].SetCurrentValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                        break;
                    }
                case ExpandDirection.Right:
                    {
                        var column = Grid.GetColumn(this);
                        _expandDistance = grid.ColumnDefinitions[column].Width;
                        grid.ColumnDefinitions[column].SetCurrentValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                        break;
                    }
                case ExpandDirection.Up:
                    {
                        var row = Grid.GetRow(this);
                        _expandDistance = grid.RowDefinitions[row].Height;
                        grid.RowDefinitions[row].SetCurrentValue(RowDefinition.HeightProperty, GridLength.Auto);
                        break;
                    }
                case ExpandDirection.Down:
                    {
                        var row = Grid.GetRow(this);
                        _expandDistance = grid.RowDefinitions[row].Height;
                        grid.RowDefinitions[row].SetCurrentValue(RowDefinition.HeightProperty, GridLength.Auto);
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

            if (!(Parent is Grid grid))
            {
                return;
            }

            switch (ExpandDirection)
            {
                case ExpandDirection.Left:
                    {
                        var column = Grid.GetColumn(this);
                        if (_expandDistance.HasValue)
                        {
                            grid.ColumnDefinitions[column].SetCurrentValue(ColumnDefinition.WidthProperty, _expandDistance.Value);
                        }
                        break;
                    }

                case ExpandDirection.Right:
                    {
                        var column = Grid.GetColumn(this);
                        if (_expandDistance.HasValue)
                        {
                            grid.ColumnDefinitions[column].SetCurrentValue(ColumnDefinition.WidthProperty, _expandDistance.Value);
                        }
                        break;
                    }

                case ExpandDirection.Up:
                    {
                        var row = Grid.GetRow(this);
                        if (_expandDistance.HasValue)
                        {
                            grid.RowDefinitions[row].SetCurrentValue(RowDefinition.HeightProperty, _expandDistance.Value);
                        }
                        break;
                    }

                case ExpandDirection.Down:
                    {
                        var row = Grid.GetRow(this);
                        if (_expandDistance.HasValue)
                        {
                            grid.RowDefinitions[row].SetCurrentValue(RowDefinition.HeightProperty, _expandDistance.Value);
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
