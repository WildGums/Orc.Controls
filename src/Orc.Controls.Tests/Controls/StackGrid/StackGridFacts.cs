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

            Assert.That(Grid.GetRow(textBox1), Is.EqualTo(0));
            Assert.That(Grid.GetRowSpan(textBox1), Is.EqualTo(1));
            Assert.That(Grid.GetColumn(textBox1), Is.EqualTo(0));
            Assert.That(Grid.GetColumnSpan(textBox1), Is.EqualTo(1));

            Assert.That(Grid.GetRow(emptyRow), Is.EqualTo(1));
            Assert.That(Grid.GetRowSpan(emptyRow), Is.EqualTo(1));
            Assert.That(Grid.GetColumn(emptyRow), Is.EqualTo(0));
            Assert.That(Grid.GetColumnSpan(emptyRow), Is.EqualTo(1));

            Assert.That(Grid.GetRow(textBox2), Is.EqualTo(2));
            Assert.That(Grid.GetRowSpan(textBox2), Is.EqualTo(1));
            Assert.That(Grid.GetColumn(textBox2), Is.EqualTo(0));
            Assert.That(Grid.GetColumnSpan(textBox2), Is.EqualTo(1));
        }
    }
}
