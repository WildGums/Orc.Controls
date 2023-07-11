namespace Orc.Controls;

using System;
using System.Threading.Tasks;
using Catel.Data;

public abstract class ControlToolBase : ModelBase, IControlTool
{
    protected object? Target;

    public abstract string Name { get; }
    public bool IsOpened { get; private set; }
    public virtual bool IsEnabled => true;
    protected virtual bool StaysOpen { get; set; } = false;

    public event EventHandler<EventArgs>? Attached;
    public event EventHandler<EventArgs>? Detached;
    public event EventHandler<EventArgs>? Closed;
    public event EventHandler<EventArgs>? Opened;
    public event EventHandler<EventArgs>? Opening;

    public virtual void Attach(object target)
    {
        Target = target;

        Attached?.Invoke(this, EventArgs.Empty);
    }

    public virtual void Detach()
    {
        Target = null;

        Detached?.Invoke(this, EventArgs.Empty);
    }

    public async Task OpenAsync(object? parameter = null)
    {
        if (IsOpened)
        {
            OnAddParameter(parameter);

            return;
        }

        Opening?.Invoke(this, EventArgs.Empty);

        await OnOpenAsync(parameter);

        IsOpened = true;
        Opened?.Invoke(this, EventArgs.Empty);

        if (!StaysOpen)
        {
            await CloseAsync();
        }
    }

    public virtual async Task CloseAsync()
    {
        IsOpened = false;

        Closed?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnAddParameter(object? parameter)
    {
    }

    protected abstract Task OnOpenAsync(object? parameter = null);
}
