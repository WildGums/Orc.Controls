namespace Orc.Controls.Settings;

using System;
using System.Windows;
using System.Windows.Markup;

/// <summary>
/// Base class for control settings adapters
/// </summary>
public abstract class ControlSettingsAdapterBase<TControl, TSettings> : MarkupExtension, IControlSettingsAdapter<TControl, TSettings>
    where TControl : FrameworkElement
    where TSettings : class
{
    protected TControl? Control { get; private set; }
    
    public event EventHandler? SettingsChanged;

    public virtual void Attach(TControl control)
    {
        if (Control is not null)
        {
            Detach();
        }
        
        Control = control ?? throw new ArgumentNullException(nameof(control));
        SubscribeToControlEvents();
    }

    public virtual void Detach()
    {
        if (Control is not null)
        {
            UnsubscribeFromControlEvents();
            Control = null;
        }
    }

    protected virtual void OnSettingsChanged()
    {
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public abstract TSettings? GetCurrentSettings();
    public abstract void ApplySettings(TSettings settings);
    public abstract bool AreSettingsEqual(TSettings? current, TSettings? saved);

    protected abstract void SubscribeToControlEvents();
    protected abstract void UnsubscribeFromControlEvents();

    // MarkupExtension implementation
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
