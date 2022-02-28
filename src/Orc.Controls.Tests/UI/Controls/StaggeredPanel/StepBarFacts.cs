namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Orc.Automation;

    [TestFixture(TestOf = typeof(StaggeredPanel))]
    [Category("UI Tests")]
    public partial class StaggeredPanelFacts : StyledControlTestFacts<StaggeredPanel>
    {
        [Target]
        public Automation.StaggeredPanel Target { get; set; }

        [Test]
        public void CorrectlyInitializeItems()
        {
            var target = Target;
            var model = target.Current;

            model.ColumnSpacing = 30;
            model.RowSpacing = 50;
            model.DesiredColumnWidth = 150;

            InitializeItems(new List<StaggeredPanelTestItem>
            {
                new () { Content = "Item_1", Width = 150, Height = 250},
                new () { Content = "Item_2", Width = 150, Height = 250},
                new () { Content = "Item_3", Width = 150, Height = 250},
                new () { Content = "Item_4", Width = 150, Height = 250},
                new () { Content = "Item_5", Width = 150, Height = 250},
                new () { Content = "Item_6", Width = 150, Height = 250},
                new () { Content = "Item_7", Width = 150, Height = 250},
            });
        }

        [Test]
        public void Correctly2()
        {
            var target = Target;
            var model = target.Current;
        }
    }
}
