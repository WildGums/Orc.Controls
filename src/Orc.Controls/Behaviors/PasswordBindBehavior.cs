// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordBindBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Catel.Windows.Interactivity;

    public class PasswordBindBehavior : BehaviorBase<PasswordBox>
    {
        #region Dependency properties
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password),
            typeof(string), typeof(PasswordBindBehavior), new PropertyMetadata(default(string), (sender, args) => ((PasswordBindBehavior)sender).OnPasswordChanged(args)));
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            AssociatedObject.PasswordChanged += OnPasswordChanged;

            base.OnAssociatedObjectLoaded();
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            AssociatedObject.PasswordChanged -= OnPasswordChanged;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnPasswordChanged(DependencyPropertyChangedEventArgs args)
        {
            var passwordTextBox = AssociatedObject;
            if (passwordTextBox is null)
            {
                return;
            }

            var newPassword = args.NewValue as string;
            if (passwordTextBox.Password == newPassword)
            {
                return;
            }

            passwordTextBox.Password = newPassword;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            SetCurrentValue(PasswordProperty, AssociatedObject.Password);
        }
        #endregion
    }
}
