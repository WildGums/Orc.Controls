namespace Orc.Controls;

using System;

public interface IPostponeActionTimer
{
    TimeSpan Interval { get; set; }
    bool IsEnabled { get; }
    event EventHandler Tick;

    void Start();
    void Stop();
}