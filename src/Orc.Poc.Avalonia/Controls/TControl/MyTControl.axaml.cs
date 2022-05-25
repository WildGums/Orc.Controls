using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Orc.Poc.Avalonia.Controls;

public class MyTControl : TemplatedControl
{
    public static readonly StyledProperty<string> TitleProperty 
        = AvaloniaProperty.Register<MyTControl, string>(nameof(Title), defaultValue: "This is title");

    public string Title
    {
        get { return GetValue(TitleProperty); }
#pragma warning disable IDISP004
        set { SetValue(TitleProperty, value); }
#pragma warning restore IDISP004
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var textBlock = e.NameScope.Find<TextBlock>("PART_TextBlock");
        textBlock.Foreground = Brushes.Red;
    }
}

