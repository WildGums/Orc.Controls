namespace Orc.Controls;

using System;
using System.Threading.Tasks;

public interface IControlTool
{
    string Name { get; }
    bool IsOpened { get; }
    bool IsEnabled { get; }
    bool IsAttached { get; }

    void Attach(object target);
    void Detach();
    Task OpenAsync(object? parameter);
    Task CloseAsync();

    event EventHandler<EventArgs>? Attached;
    event EventHandler<EventArgs>? Detached;
    event EventHandler<EventArgs>? Opening;
    event EventHandler<EventArgs>? Opened;
    event EventHandler<EventArgs>? Closed;
}
