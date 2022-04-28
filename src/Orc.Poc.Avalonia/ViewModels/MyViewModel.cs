namespace Orc.Poc.Avalonia.ViewModels;

using System.Collections.ObjectModel;

public class MyViewModel : ViewModelBase
{
    private readonly IMyModelProvider _myModelProvider;
    
    public MyViewModel(IMyModelProvider myModelProvider)
    {
        _myModelProvider = myModelProvider;

        Items = new ObservableCollection<MyModel>(_myModelProvider.GetMyModels());
    }
    public ObservableCollection<MyModel> Items { get; }
}
