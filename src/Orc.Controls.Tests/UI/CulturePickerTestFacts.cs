namespace Orc.Controls.Tests
{
    using System.Threading;
    using System.Windows.Automation;
    using Automation;
    using Automation.Tests;
    using NUnit.Framework;
    using UI;

    [TestFixture]
    public class CulturePickerTestFacts : UiTestFactsBase<OrcControlsUiTestModel>
    {
        //private AutomationElement _culturePicker;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            var model = TestModel;

            var window = model.MainWindow;

            var tabControl = window.Find<TabControlAutomationElement>(id: "TabControl");
            tabControl.SelectItem("CulturePicker");
            
            var culturePickerView = tabControl.Element.Find(id: "CulturePickerViewId");

            var child = TreeWalker.ControlViewWalker.GetFirstChild(culturePickerView);
            
            var culturePicker = culturePickerView?.Find<ComboBoxAutomationElement>(id: "CulturePickerId");

            culturePicker?.Expand();

            Thread.Sleep(200);
        }

        [SetUp]
        public virtual void SetUpTest()
        {
          
        }

        [Test]
        public void Test()
        {
            
        }
    }
}
