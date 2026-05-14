namespace Orc.Controls.ViewModels;

using System;
using Catel.MVVM;

public partial class TextInputViewModel : ViewModelBase
{
    private readonly string _title;

    public TextInputViewModel(string? title, IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        ValidateUsingDataAnnotations = false;

        _title = title ?? string.Empty;
    }

    public override string Title => _title;
    public string? Text { get; set; }
}
