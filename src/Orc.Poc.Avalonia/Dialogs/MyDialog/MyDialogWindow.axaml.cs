using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orc.Poc.Avalonia.MyDialog;

public partial class MyDialogWindow : Window
{
    public MyDialogWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

