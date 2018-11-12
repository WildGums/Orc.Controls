// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoNotShowDropDownOnClickComboboxBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.Windows.Interactivity;

    public class DoNotShowDropDownOnClickComboboxBehavior : BehaviorBase<ComboBox>
    {
        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            var combobox = AssociatedObject;

            combobox.PreviewMouseDown += OnPreviewMouseDown;
            combobox.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            var combobox = AssociatedObject;

            combobox.PreviewMouseDown -= OnPreviewMouseDown;
            combobox.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            var originalSource = args.OriginalSource;
            if (originalSource is Border)
            {
                Keyboard.Focus(AssociatedObject);
                args.Handled = true;
            }
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            var originalSource = args.OriginalSource;
            if (originalSource is Border)
            {
                AssociatedObject.IsDropDownOpen = true;
                args.Handled = true;
            }
        }
        #endregion
    }
}
