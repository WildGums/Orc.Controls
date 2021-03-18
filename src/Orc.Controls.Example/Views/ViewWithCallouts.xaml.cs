namespace Orc.Controls.Example.Views
{
    using System;
    using System.Collections.Generic;
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for ViewWithCallouts.xaml
    /// </summary>
    public partial class ViewWithCallouts : UserControl
    {
        public ViewWithCallouts()
        {
            InitializeComponent();
        }

        public ViewWithCalloutsViewModel ViewWithCalloutsViewModel
        {
            get { return (ViewWithCalloutsViewModel)GetValue(ViewWithCalloutsViewModelProperty); }
            set { SetValue(ViewWithCalloutsViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewWithCalloutsViewModelProperty = DependencyProperty.Register(nameof(ViewWithCalloutsViewModel), typeof(ViewWithCalloutsViewModel),
            typeof(ViewWithCallouts), new PropertyMetadata(null, (sender, e) => ((ViewWithCallouts)sender).OnViewWithCalloutsViewModelChanged()));



        private void OnViewWithCalloutsViewModelChanged()
        {

        }
    }
}
