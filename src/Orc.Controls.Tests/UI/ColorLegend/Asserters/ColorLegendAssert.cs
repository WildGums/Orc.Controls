namespace Orc.Controls.Tests
{
    using Automation;
    using NUnit.Framework;

    public class ColorLegendAssert
    {
        public static void AllCheckedState(ColorLegendAutomationControl target)
        {
            var isAllChecked = target.View.IsAllChecked;

            if (isAllChecked is null)
            {
                Assert.That(target.ItemsSource, 
                    Has.Some.Property(nameof(IColorLegendItem.IsChecked)).True
                    .And
                    .Some.Property(nameof(IColorLegendItem.IsChecked)).False);
            }
            
            switch (isAllChecked)
            {
                case true:
                    Assert.That(target.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).True);
                    break;

                case false:
                    Assert.That(target.ItemsSource, Has.All.Property(nameof(IColorLegendItem.IsChecked)).False);
                    break;
            }
        }
    }
}
