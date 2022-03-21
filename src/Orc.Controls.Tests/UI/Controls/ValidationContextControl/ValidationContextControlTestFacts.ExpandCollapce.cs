namespace Orc.Controls.Tests
{
    using Automation;
    using NUnit.Framework;

    public partial class ValidationContextControlTestFacts
    {
        [Test]
        public void VerifyExpandCollapseAllState()
        {
            var target = Target;
            var model = target.Current;

            model.ValidationContext = CreateTestValidationContext();

            //first expand all nodes
            target.IsExpanded = true;

            //Then collapse all root nodes one by one
            foreach (var item in target.TabItems)
            {
                item.IsExpanded = false;
            }

            //The all button should be collapsed state
            Assert.That(target.IsExpanded, Is.EqualTo(false));
        }

        [Test]
        public void CorrectlyExpandCollapseAll()
        {
            var target = Target;
            var model = target.Current;

            model.ValidationContext = CreateTestValidationContext();

            //Collapse all items
            target.IsExpanded = false;

            Assert.That(target.TabItems, Has.All.Property(nameof(ValidationContextTagTreeItem.IsExpanded)).False);
        }
    }
}
