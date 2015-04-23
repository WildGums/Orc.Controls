// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonBehavior.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Behavior
{
    using System.Reflection;
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
            AssociatedObject.ToggleButton.IsChecked = AssociatedObject.DropDown.IsOpen;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var dropDown = AssociatedObject.DropDown;
            if (dropDown != null && (AssociatedObject.ToggleButton.IsChecked ?? false))
            {
                dropDown.Dispatcher.BeginInvoke(() =>
                {
                    var isLeft = SystemParameters.MenuDropAlignment;
                    var type = typeof(SystemParameters);
                    var menuDropAlignment = type.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);

                    if (isLeft && menuDropAlignment != null)
                    {
                        menuDropAlignment.SetValue(null, false);
                    }

                    try
                    {
                        dropDown.PlacementTarget = AssociatedObject;
                        dropDown.Placement = PlacementMode.RelativePoint;
                        dropDown.VerticalOffset = AssociatedObject.ActualHeight;
                    }
                    finally
                    {
                        if (isLeft && menuDropAlignment != null)
                        {
                            menuDropAlignment.SetValue(null, true);
                        }
                    }
                    
                });

                dropDown.IsOpen = true;
            }
        }
        #endregion
    }
}