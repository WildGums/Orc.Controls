namespace Orc.Controls.Example.Views
{
    using System.Windows;
    using Automation;

    public partial class Expander
    {
        public Expander()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var expanderControl = new ExpanderAutomationPeer(ExpanderControl);

            expanderControl.Expand();
        }
    }
}
