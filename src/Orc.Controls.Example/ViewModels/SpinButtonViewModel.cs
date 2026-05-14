namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using Catel.MVVM;

public partial class SpinButtonViewModel : ViewModelBase
{
    private readonly List<string> _allText = new()
    {
        "Olivia",
        "Emma",
        "Ava",
        "Charlotte",
        "Sophia",
        "Amelia",
        "Isabella",
        "Mia"
    };

    public SpinButtonViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _allText.Sort();

        ShowNextName = new Command<string>(serviceProvider, OnShowNextName, CanShowNextName);
        ShowPreviousName = new Command<string>(serviceProvider, OnShowPreviousName, CanShowPreviousName);

        Text = _allText[0];
    }

    public string Text { get; set; }

    public Command<string> ShowNextName { get; }

    private void OnShowNextName(string name)
    {
        var nameIndex = _allText.IndexOf(name);
        if (nameIndex < 0 || nameIndex >= _allText.Count - 1)
        {
            return;
        }

        Text = _allText[nameIndex + 1];
    }

    private bool CanShowNextName(string name)
    {
        var nameIndex = _allText.IndexOf(name);
        return nameIndex >= 0 && nameIndex < _allText.Count - 1;
    }

    public Command<string> ShowPreviousName { get; }

    private void OnShowPreviousName(string name)
    {
        var nameIndex = _allText.IndexOf(name);
        if (nameIndex <= 0)
        {
            return;
        }

        Text = _allText[nameIndex - 1];
    }

    private bool CanShowPreviousName(string name)
    {
        var nameIndex = _allText.IndexOf(name);
        return nameIndex > 0;
    }
}
