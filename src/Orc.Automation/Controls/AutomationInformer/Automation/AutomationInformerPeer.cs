namespace Orc.Automation
{
    using System.Windows;

    public class AutomationInformerPeer : RunMethodAutomationPeerBase
    {
        private readonly AutomationInformer _owner;

        public AutomationInformerPeer(AutomationInformer owner)
            : base(owner)
        {
            _owner = owner;
        }

        //[AutomationMethod]
        //public AutomationValue PutControl(string controlTypeFullName)
        //{
        //    return _testHost.PutControl(controlTypeFullName);
        //}
    }
}
