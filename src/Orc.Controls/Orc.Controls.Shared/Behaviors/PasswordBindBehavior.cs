// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordBindBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Catel.Windows.Interactivity;

    public class PasswordBindBehavior : BehaviorBase<PasswordBox>
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password", typeof (string), typeof (PasswordBindBehavior), new PropertyMetadata(default(string)));

        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;

            base.OnAssociatedObjectLoaded();
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            SetCurrentValue(PasswordProperty, AssociatedObject.Password);
        }
    }
}