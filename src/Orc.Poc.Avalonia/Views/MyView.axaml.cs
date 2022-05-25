using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Orc.Poc.Avalonia.Views;

using Catel.IoC;
using ViewModels;

public partial class MyView : UserControl
{
    public MyView()
    {
        InitializeComponent();

        var typeFactory = this.GetTypeFactory();
        
        DataContext = typeFactory.CreateInstanceWithParametersAndAutoCompletion<MyViewModel>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
