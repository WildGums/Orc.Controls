// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Behavior
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using Catel.Windows.Interactivity;
    using Catel.Windows.Threading;

    public class DropDownButtonBehavior : BehaviorBase<DropDownButton>
    {
        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            var binding = new Binding("DropDown.IsOpen")
            {
                Source = AssociatedObject,
                Mode = BindingMode.TwoWay
            };

            AssociatedObject.SetBinding(ToggleButton.IsCheckedProperty, binding);

            AssociatedObject.ToggleButton.Click += OnClick;

            var dropDown = AssociatedObject.DropDown;
            if (dropDown != null)
            {
                dropDown.Closed += OnDropDownClosed;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            base.OnAssociatedObjectUnloaded();

            AssociatedObject.ToggleButton.Click -= OnClick;

            var dropDown = AssociatedObject.DropDown;
            if (dropDown != null)
            {
                dropDown.Closed -= OnDropDownClosed;
            }
        }

        private void OnDropDownClosed(object sender, RoutedEventArgs e)
        {
            AssociatedObject.ToggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, AssociatedObject.DropDown.IsOpen);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var dropDown = AssociatedObject.DropDown;
            if (dropDown == null || (!(AssociatedObject.ToggleButton.IsChecked ?? false)))
            {
                return;
            }

            dropDown.Dispatcher.BeginInvoke(() =>
            {
                dropDown.SetCurrentValue(System.Windows.Controls.ContextMenu.PlacementTargetProperty, AssociatedObject);
                dropDown.SetCurrentValue(System.Windows.Controls.ContextMenu.PlacementProperty, PlacementMode.Custom);
                dropDown.SetCurrentValue(FrameworkElement.MinWidthProperty, AssociatedObject.ActualWidth);
                dropDown.SetCurrentValue(System.Windows.Controls.ContextMenu.CustomPopupPlacementCallbackProperty, (System.Windows.Controls.Primitives.CustomPopupPlacementCallback)CustomPopupPlacementCallback);
            });

            dropDown.SetCurrentValue(System.Windows.Controls.ContextMenu.IsOpenProperty, true);
        }

        private static CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
        {
            var p = new Point
            {
                Y = targetSize.Height - offset.Y,
                X = -offset.X
            };

            return new[]
            {
                new CustomPopupPlacement(p, PopupPrimaryAxis.Horizontal)
            };
        }
        #endregion
    }
}
