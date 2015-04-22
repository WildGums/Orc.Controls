// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonBehavior.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Behavior
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using Catel.Windows.Interactivity;

    public class DropDownButtonBehavior : BehaviorBase<DropDownButton>
    {
        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();
            var binding = new Binding("DropDown.IsOpen") {Source = AssociatedObject, Mode = BindingMode.TwoWay};
            AssociatedObject.SetBinding(ToggleButton.IsCheckedProperty, binding);

            AssociatedObject.ToggleButton.Click += OnClick;

            if (AssociatedObject.DropDown == null)
            {
                return;
            }

            AssociatedObject.DropDown.Closed += OnDropDownClosed;
        }

        private void OnDropDownClosed(object sender, RoutedEventArgs e)
        {
            AssociatedObject.ToggleButton.IsChecked = AssociatedObject.DropDown.IsOpen;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DropDown != null && (AssociatedObject.ToggleButton.IsChecked ?? false))
            {
                AssociatedObject.DropDown.PlacementTarget = AssociatedObject;
                AssociatedObject.DropDown.VerticalOffset = AssociatedObject.Height;
                AssociatedObject.DropDown.Placement = PlacementMode.RelativePoint;

                AssociatedObject.DropDown.IsOpen = true;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            base.OnAssociatedObjectUnloaded();
            AssociatedObject.ToggleButton.Click -= OnClick;

            if (AssociatedObject.DropDown == null)
            {
                return;
            }

            AssociatedObject.DropDown.Closed -= OnDropDownClosed;
        }
        #endregion
    }
}