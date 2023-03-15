namespace Orc.Controls;

using System;

public interface IControlTool
{
    string Name { get; }
    bool IsOpened { get; }
    bool IsEnabled { get; }

    void Attach(object target);
    void Detach();
    void Open(object? parameter);
    void Close();

    event EventHandler<EventArgs>? Attached;
    event EventHandler<EventArgs>? Detached;
    event EventHandler<EventArgs>? Opening;
    event EventHandler<EventArgs>? Opened;
    event EventHandler<EventArgs>? Closed;
}
