namespace Orc.Controls.Views;

using Catel.Windows;

public partial class TextInputWindow
{
    partial void OnInitializingComponent()
    {
        Mode = DataWindowMode.OkCancel;
    }
}
