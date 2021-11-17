namespace Orc.Automation
{
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

        [CommandRunMethod]
        public bool LoadResources(string uri)
        {
            return _testHost.LoadResources(uri);
        }

        [CommandRunMethod]
        public bool LoadAssembly(string location)
        {
            var assembly = _testHost.LoadAssembly(location);

            return assembly is not null;
        }
    }
}
