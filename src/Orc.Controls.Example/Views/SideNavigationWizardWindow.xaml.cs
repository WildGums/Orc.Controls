namespace Orc.Controls.Example.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using Catel.Windows;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for SideNavigationWizardWindow.xaml
    /// </summary>
    public partial class SideNavigationWizardWindow
    {
        public SideNavigationWizardWindow()
        {
            InitializeComponent();
        }

        //public SideNavigationWizardWindow()
        //    : this(null)
        //{
        //}

        //public SideNavigationWizardWindow(SideNavigationWizardViewModel viewModel)
        //    : base(viewModel, DataWindowMode.Custom, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.Overlay)
        //{
        //    InitializeComponent();
        //}

        //protected override void OnLoaded(EventArgs e)
        //{
        //    base.OnLoaded(e);

        //    Dispatcher.BeginInvoke((Action)(() =>
        //    {
        //        UpdateOpacityMask();
        //    }));
        //}

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            //if (ViewModel is SideNavigationWizardViewModel vm)
            //{
            //    if (vm.Wizard is SideNavigationWizardBase sideNavigationWizard)
            //    {
            //        if (sideNavigationWizard.ShowFullScreen)
            //        {
            //            SetCurrentValue(WindowStateProperty, WindowState.Maximized);
            //            SetCurrentValue(WindowStyleProperty, WindowStyle.None);
            //            SetCurrentValue(BorderThicknessProperty, new Thickness(0));
            //        }
            //    }
            //}
        }

//        protected override void OnViewModelPropertyChanged(PropertyChangedEventArgs e)
//        {
//            base.OnViewModelPropertyChanged(e);



//#pragma warning disable WPF1014
//            if (e.HasPropertyChanged("CurrentPage"))
//#pragma warning restore WPF1014
//            {
//#pragma warning disable AvoidAsyncVoid
//                Dispatcher.BeginInvoke((Action)(async () =>
//                {
//#pragma warning restore AvoidAsyncVoid
//                    var vm = (SideNavigationWizardViewModel)ViewModel;
//                    //ListBox stepbarListBox = (ListBox)stepBarItem.GetType().GetField("listBox").GetValue(this);
//                    //ProgressBar stepbarProgress = (ProgressBar)stepBarItem.GetType().GetField("progressBar").GetValue(this);

//                    stepbar.stepbarListBox.CenterSelectedItem();
//                    stepbar.stepbarProgress.UpdateProgress(vm.Wizard.CurrentPage.Number, vm.Wizard.Pages.Count());

//                    // We need to await the animation
//                    await TaskShim.Delay(WizardConfiguration.AnimationDuration);

//                    UpdateOpacityMask();
//                }));
//            }
//        }

//        private void UpdateOpacityMask()
//        {
//            var scrollViewer = stepbar.FindVisualDescendantByType<ScrollViewer>();
//            if (scrollViewer is null)
//            {
//                return;
//            }

//            var opacityMask = new LinearGradientBrush();
//            if (scrollViewer.HorizontalOffset > 0d)
//            {
//                opacityMask.GradientStops.Add(new GradientStop(Colors.Transparent, 0d));
//                opacityMask.GradientStops.Add(new GradientStop(Colors.Black, 0.05d));
//            }

//            var scrollableWidth = scrollViewer.ScrollableWidth;
//            if (scrollableWidth > scrollViewer.HorizontalOffset)
//            {
//                opacityMask.GradientStops.Add(new GradientStop(Colors.Black, 0.95d));
//                opacityMask.GradientStops.Add(new GradientStop(Colors.Transparent, 1d));
//            }

//            stepbar.SetCurrentValue(OpacityMaskProperty, opacityMask.GradientStops.Count > 0 ? opacityMask : null);
//        }
    }
}

