namespace Orc.Controls.Example.Views
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Threading;
    using Catel.Windows;
    using Catel.Windows.Threading;
    using ControlzEx.Behaviors;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SideNavigationWizardWindow.xaml
    /// </summary>
    public partial class HorizontalNavigationWizardView
    {
        public HorizontalNavigationWizardView()
        {
            InitializeComponent();
        }

        public HorizontalNavigationWizardView(HorizontalNavigationWizardViewModel viewModel)
        {
            InitializeComponent();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            stepbar.MoveForwardAsync();
        }

        private void Previous_Button_Click(object sender, RoutedEventArgs e)
        {
            stepbar.MoveBackAsync();
        }
    }
}

