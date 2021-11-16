namespace Orc.Controls.Controls.Automation
{
    using System.Windows.Automation.Peers;
    using Orc.Controls.Automation;

    public class TestHostAutomationPeer : CommandAutomationPeerBase
    {
        private readonly TestHost _testHost;

        public TestHostAutomationPeer(TestHost owner) 
            : base(owner)
        {
            _testHost = owner;
        }

        [CommandRunMethod]
        public string PutControl(string controlTypeFullName)
        {
            return _testHost.PutControl(controlTypeFullName);
        }
    }
}
