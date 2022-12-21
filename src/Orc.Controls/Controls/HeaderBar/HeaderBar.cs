namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using Automation;

    public class HeaderBar : Control
    {
        #region Constructors
        public HeaderBar()
        {
            DefaultStyleKey = typeof(HeaderBar);
        }
        #endregion

        #region Properties
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
            typeof(string), typeof(HeaderBar), new PropertyMetadata(string.Empty));
        #endregion

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new HeaderBarAutomationPeer(this);
        }
    }
}
