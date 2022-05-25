using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orc.Poc.Avalonia.Controls;

using System.Linq;
using global::Avalonia.Interactivity;
using global::Avalonia.VisualTree;
using MyDialog;

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

    private async void Button_OnClick(object sender, RoutedEventArgs e)
    {
        // if (sender is not Button button)
        // {
        //     return;
        // }
        //
        // var mainWindow = button.GetVisualAncestors().FirstOrDefault(x => x is Window) as Window;
        //
        // var dialogWindow = new MyDialogWindow();
        // await dialogWindow.ShowDialog(mainWindow);
    }
}

