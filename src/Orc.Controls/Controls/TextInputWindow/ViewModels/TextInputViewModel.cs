namespace Orc.Controls.ViewModels;

using Catel.MVVM;

public class TextInputViewModel : ViewModelBase
{
    private readonly string _title;

    public TextInputViewModel(string? title)
    {
        _title = title ?? string.Empty;
    }

    public override string Title => _title;
    public string? Text { get; set; }
}
