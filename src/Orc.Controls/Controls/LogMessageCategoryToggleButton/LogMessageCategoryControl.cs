namespace Orc.Controls;

using System.Windows;
using System.Windows.Controls;

public partial class LogMessageCategoryControl : Control
{
    public string? Category
    {
        get { return (string?)GetValue(CategoryProperty); }
        set { SetValue(CategoryProperty, value); }
    }

    public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(nameof(Category),
        typeof(string), typeof(LogMessageCategoryControl), new PropertyMetadata(string.Empty));
}
