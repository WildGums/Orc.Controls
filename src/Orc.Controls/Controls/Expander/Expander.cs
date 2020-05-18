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
    using System.Windows.Media.Animation;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    
    [TemplateVisualState(Name = "Expanded", GroupName = "Expander")]
    [TemplateVisualState(Name = "Collapsed", GroupName = "Expander")]
    [TemplatePart(Name = "PART_ExpandSite", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_HeaderSiteBorder", Type = typeof(Border))]
    public class Expander : HeaderedContentControl
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private GridLength? _expandDistance;
        private double? _previousActualLength;
        private double? _previousMaxLenght;
        private double? _expanderContentLenght;
        private ContentPresenter _expandSite;
        private Border _headerSiteBorder;
        #endregion

        #region Constructors
        public Expander()
        {
            DefaultStyleKey = typeof(Expander);

            _dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
        }
        #endregion

        #region Properties
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof(IsExpanded),
            typeof(bool), typeof(Expander),
            new PropertyMetadata(true, (sender, args) => ((Expander)sender).OnIsExpandedChanged(args)));

        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(nameof(ExpandDirection),
            typeof(ExpandDirection), typeof(Expander), new PropertyMetadata(ExpandDirection.Left));
        
        public Duration ExpandDuration
        {
            get { return (Duration)GetValue(ExpandDurationProperty); }
            set { SetValue(ExpandDurationProperty, value); }
        }

        public static readonly DependencyProperty ExpandDurationProperty = DependencyProperty.Register(
            nameof(ExpandDuration), typeof(Duration), typeof(Expander), new PropertyMetadata(new Duration(new TimeSpan(0, 0, 0, 0, 250))));


        public bool AutoResizeGrid
        {
            get { return (bool)GetValue(AutoResizeGridProperty); }
            set { SetValue(AutoResizeGridProperty, value); }
        }

        public static readonly DependencyProperty AutoResizeGridProperty = DependencyProperty.Register(nameof(AutoResizeGrid),
            typeof(bool), typeof(Expander), new PropertyMetadata(false));
        #endregion

        #region Methods
        private void OnIsExpandedChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                OnExpanded();
            }
            else
            {
                OnCollapsed();
            }

            UpdateStates(true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _expandSite = GetTemplateChild("PART_ExpandSite") as ContentPresenter;
            if (_expandSite is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_ExpandSite'");
            }

            _headerSiteBorder = GetTemplateChild("PART_HeaderSiteBorder") as Border;
            if (_headerSiteBorder is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HeaderSiteBorder'");
            }
            
            UpdateStates(false);
        }

        private void AnimateMaxWidth(ColumnDefinition columnDefinition, double from, double to, double duration)
        {

            _dispatcherService.Invoke(() =>
            {
                var storyboard = new Storyboard();

                var anumationDuration = new Duration(TimeSpan.FromMilliseconds(duration));
                var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

                var animation = new DoubleAnimation { EasingFunction = ease, Duration = anumationDuration };
                storyboard.Children.Add(animation);
                animation.From = from;
                animation.To = to;
                Storyboard.SetTarget(animation, columnDefinition);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

                storyboard.Completed += StoryboardOnCompleted;

                storyboard.Begin();
            });

        }
        private void StoryboardOnCompleted(object sender, EventArgs e)
        {
            var isExpanded = IsExpanded;

            if (!isExpanded)
            {
                return;
            }

            if (_previousMaxLenght is null)
            {
                return;
            }

            if (!AutoResizeGrid)
            {
                return;
            }

            if (!(Parent is Grid grid))
            {
                return;
            }

            var column = Grid.GetColumn(this);
            var columnDefinition = grid.ColumnDefinitions[column];

            columnDefinition.BeginAnimation(ColumnDefinition.MaxWidthProperty, null);
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
                        _expanderContentLenght = _expandSite.ActualWidth;

                        var columnDefinition = grid.ColumnDefinitions[column];

                        if (_previousMaxLenght is null)
                        {
                            _previousMaxLenght = columnDefinition.MaxWidth;
                        }
                        
                        _previousActualLength = columnDefinition.ActualWidth;
                        AnimateMaxWidth(columnDefinition, columnDefinition.ActualWidth, _headerSiteBorder.ActualWidth, 250d);

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

        private IDispatcherService _dispatcherService;

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

                        if (_previousActualLength.HasValue)
                        {
                            var columnDefinition = grid.ColumnDefinitions[column];
                            AnimateMaxWidth(columnDefinition, _headerSiteBorder.ActualWidth, _previousActualLength.Value, 250d);

                            //_expandSite.Width = _expanderContentLenght.Value;
                            //grid.ColumnDefinitions[column].SetCurrentValue(ColumnDefinition.WidthProperty, GridLength.Auto);

                            //_timer.Stop();
                            //_timer.Start();
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

        private void UpdateStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsExpanded ? "Expanded" : "Collapsed", useTransitions);
        }
        #endregion
    }
}
