namespace Orc.Controls;

using System;

public partial class LogFilterGroupList
{
    private LogFilterGroupListViewModel? _lastKnownViewModel;

    public LogFilterGroupList() => InitializeComponent();
    
    protected override void OnViewModelChanged()
    {
        base.OnViewModelChanged();

        if (_lastKnownViewModel is not null)
        {
            _lastKnownViewModel.Updated -= OnViewModelUpdated;
            _lastKnownViewModel = null;
        }

        _lastKnownViewModel = ViewModel as LogFilterGroupListViewModel;
        if (_lastKnownViewModel is not null)
        {
            _lastKnownViewModel.Updated += OnViewModelUpdated;
        }
    }

    private void OnViewModelUpdated(object? sender, EventArgs e)
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<EventArgs>? Updated;
}
