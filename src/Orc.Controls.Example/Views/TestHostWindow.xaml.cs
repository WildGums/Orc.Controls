namespace Orc.Controls.Example.Views
{
    using System.Windows.Automation.Peers;
    using Catel.Windows;
    using Window = System.Windows.Window;

    public class TestHostWindowAutomationPeer : WindowAutomationPeer
    {
        public TestHostWindowAutomationPeer(Window owner) 
            : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return typeof(TestHostWindow).FullName;
        }
    }

    public partial class TestHostWindow
    {
        public TestHostWindow()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new TestHostWindowAutomationPeer(this);
        }
    }
}
