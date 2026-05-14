namespace Orc.Controls.Example.ViewModels;

using System;
using System.Collections.Generic;
using Catel.MVVM;

public partial class FilterBoxViewModel : ViewModelBase
{
    public FilterBoxViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        FilterSource = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("1", "abcd"),
            new KeyValuePair<string, string>("2", "basdf"),
            new KeyValuePair<string, string>("3", "bwerr"),
            new KeyValuePair<string, string>("4", "oiydd"),
            new KeyValuePair<string, string>("5", "klhhs"),
            new KeyValuePair<string, string>("6", "sdfhi"),
        };
    }

    public List<KeyValuePair<string, string>> FilterSource { get; }
    public string FilterText { get; set; }
    public string CustomServiceFilterText { get; set; }
}
