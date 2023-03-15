namespace Orc.Controls.ViewModels;

using Catel.MVVM;

public class TextInputViewModel : ViewModelBase
{
    public TextInputViewModel(string title)
    {
        Title = title;
    }

    public string? Text { get; set; }
}
