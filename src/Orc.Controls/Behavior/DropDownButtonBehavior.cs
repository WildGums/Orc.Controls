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
            var binding = new Binding("DropDown.IsOpen") {Source = AssociatedObject.ToggleButton};
            AssociatedObject.SetBinding(ToggleButton.IsCheckedProperty, binding);

            AssociatedObject.ToggleButton.Click += OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.DropDown != null && (AssociatedObject.ToggleButton.IsChecked ?? false))
            {
                AssociatedObject.DropDown.PlacementTarget = AssociatedObject;
                AssociatedObject.DropDown.Placement = PlacementMode.Bottom;

                AssociatedObject.DropDown.IsOpen = true;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            base.OnAssociatedObjectUnloaded();
            AssociatedObject.ToggleButton.Click -= OnClick;
        }
        #endregion
    }
}