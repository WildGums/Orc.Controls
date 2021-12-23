namespace Orc.Automation
{
    public class TestHostAutomationPeer : RunMethodAutomationPeerBase
    {
        private readonly TestHost _testHost;

        public TestHostAutomationPeer(TestHost owner) 
            : base(owner)
        {
            _testHost = owner;
        }

        [AutomationMethod]
        public string PutControl(string controlTypeFullName)
        {
            return _testHost.PutControl(controlTypeFullName);
        }

        [AutomationMethod]
        public bool LoadResources(string uri)
        {
            return _testHost.LoadResources(uri);
        }

        [AutomationMethod]
        public bool LoadAssembly(string location)
        {
            var assembly = _testHost.LoadAssembly(location);

            return assembly is not null;
        }

        [AutomationMethod]
        public void LoadUnmanaged(string location)
        {
            _testHost.LoadUnmanaged(location);
        }

        [AutomationMethod]
        public object GetResource(string name)
        {
            return _testHost.TryFindResource(name);
        }
    }
}
