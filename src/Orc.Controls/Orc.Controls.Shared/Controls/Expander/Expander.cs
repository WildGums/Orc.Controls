// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Expander.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public enum ExpandDirection
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3,
    }

    public class Expander : HeaderedContentControl
    {
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof (bool), typeof (Expander), new PropertyMetadata(false,
                OnIsExpandedPropertyChanged));

        public static readonly DependencyProperty ExpandDirectionProperty =
            DependencyProperty.Register("ExpandDirection", typeof (ExpandDirection), typeof (Expander), new PropertyMetadata(ExpandDirection.Left));

        public static readonly DependencyProperty AutoResizeGridProperty =
            DependencyProperty.Register("AutoResizeGrid", typeof (bool), typeof (Expander), new PropertyMetadata(false));

        #region Fields
        private GridLength previousValue;
        #endregion

        #region Constructors
        public Expander()
        {
            this.DefaultStyleKey = typeof (Expander);
        }
        #endregion

        #region Properties
        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection) GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public bool AutoResizeGrid
        {
            get { return (bool) GetValue(AutoResizeGridProperty); }
            set { SetValue(AutoResizeGridProperty, value); }
        }
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

            if (this.Parent is Grid)
            {
                var grid = this.Parent as Grid;
                switch (this.ExpandDirection)
                {
                    case ExpandDirection.Left:
                    {
                        var column = Grid.GetColumn(this);
                        previousValue = grid.ColumnDefinitions[column].Width;
                        grid.ColumnDefinitions[column].Width = GridLength.Auto;
                        break;
                    }
                    case ExpandDirection.Right:
                    {
                        var column = Grid.GetColumn(this);
                        previousValue = grid.ColumnDefinitions[column].Width;
                        grid.ColumnDefinitions[column].Width = GridLength.Auto;
                        break;
                    }
                    case ExpandDirection.Up:
                    {
                        var row = Grid.GetRow(this);
                        previousValue = grid.RowDefinitions[row].Height;
                        grid.RowDefinitions[row].Height = GridLength.Auto;
                        break;
                    }
                    case ExpandDirection.Down:
                    {
                        var row = Grid.GetRow(this);
                        previousValue = grid.RowDefinitions[row].Height;
                        grid.RowDefinitions[row].Height = GridLength.Auto;
                        break;
                    }
                }
            }
        }

        protected virtual void OnExpanded()
        {
            if (!AutoResizeGrid)
            {
                return;
            }

            if (Parent is Grid)
            {
                var grid = Parent as Grid;

                switch (this.ExpandDirection)
                {
                    case ExpandDirection.Left:
                    {
                        var column = Grid.GetColumn(this);
                        if (previousValue != null)
                        {
                            grid.ColumnDefinitions[column].Width = previousValue;
                        }
                        break;
                    }
                    case ExpandDirection.Right:
                    {
                        var column = Grid.GetColumn(this);
                        if (previousValue != null)
                        {
                            grid.ColumnDefinitions[column].Width = previousValue;
                        }
                        break;
                    }
                    case ExpandDirection.Up:
                    {
                        var row = Grid.GetRow(this);
                        if (previousValue != null)
                        {
                            grid.RowDefinitions[row].Height = previousValue;
                        }
                        break;
                    }
                    case ExpandDirection.Down:
                    {
                        var row = Grid.GetRow(this);
                        if (previousValue != null)
                        {
                            grid.RowDefinitions[row].Height = previousValue;
                        }
                        break;
                    }
                }
            }
        }
        #endregion
    }
}