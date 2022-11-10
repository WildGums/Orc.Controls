namespace Orc.Controls.Tests.Controls
{
    using System.Windows.Controls;
    using NUnit.Framework;

    [TestFixture, RequiresThread(System.Threading.ApartmentState.STA)]
    public class StackGridFacts
    {
        [TestCase]
        public void Sets_SetColumnsAndRows_Without_ColumnDefinitions()
        {
            var stackGrid = new StackGrid();

            stackGrid.RowDefinitions.Add(new RowDefinition());
            stackGrid.RowDefinitions.Add(new RowDefinition());
            stackGrid.RowDefinitions.Add(new RowDefinition());

            var textBox1 = new TextBox();
            stackGrid.Children.Add(textBox1);

            var emptyRow = new EmptyRow();
            stackGrid.Children.Add(emptyRow);

            var textBox2 = new TextBox();
            stackGrid.Children.Add(textBox2);

            stackGrid.SetColumnsAndRows();

            Assert.AreEqual(0, Grid.GetRow(textBox1));
            Assert.AreEqual(1, Grid.GetRowSpan(textBox1));
            Assert.AreEqual(0, Grid.GetColumn(textBox1));
            Assert.AreEqual(1, Grid.GetColumnSpan(textBox1));

            Assert.AreEqual(1, Grid.GetRow(emptyRow));
            Assert.AreEqual(1, Grid.GetRowSpan(emptyRow));
            Assert.AreEqual(0, Grid.GetColumn(emptyRow));
            Assert.AreEqual(1, Grid.GetColumnSpan(emptyRow));

            Assert.AreEqual(2, Grid.GetRow(textBox2));
            Assert.AreEqual(1, Grid.GetRowSpan(textBox2));
            Assert.AreEqual(0, Grid.GetColumn(textBox2));
            Assert.AreEqual(1, Grid.GetColumnSpan(textBox2));
        }
    }
}
