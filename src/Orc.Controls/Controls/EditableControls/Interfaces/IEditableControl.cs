namespace Orc.Controls;

using System;

public interface IEditableControl
{
    bool IsInEditMode { get; }

    event EventHandler<EventArgs>? EditStarted;
    event EventHandler<EventArgs>? EditEnded;
}
