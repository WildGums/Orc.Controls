using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orc.Poc.Avalonia.Controls;

public partial class MyControl : UserControl
{
    public MyControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

