namespace Orc.Controls;

using System;
using System.Windows.Input;

public interface ICallout
{
    Guid Id { get; }

    string? Name { get; }

    string Title { get; }

    object? Tag { get; }

    bool IsOpen { get; }

    bool IsClosable { get; }

    bool HasShown { get; }

    string? Version { get; }

    TimeSpan ShowTime { get; set; }

    ICommand? Command { get; }

    void Show();
    void Hide();

    event EventHandler<CalloutEventArgs>? Showing;
    event EventHandler<CalloutEventArgs>? Hiding;
}
