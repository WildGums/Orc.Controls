namespace Orc.Controls;

using System;
using System.Windows;
using Catel.Windows.Interactivity;

public abstract class EditableControlBehaviorBase<T> : BehaviorBase<T>, IEditableControl
    where T : FrameworkElement
{
    #region Dependency properties
    public bool IsInEditMode
    {
        get { return (bool)GetValue(IsInEditModeProperty); }
        set { SetValue(IsInEditModeProperty, value); }
    }

    public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(
        nameof(IsInEditMode), typeof(bool), typeof(EditableControlBehaviorBase<T>), new PropertyMetadata(default(bool)));
    #endregion

    public event EventHandler<EventArgs>? EditStarted;
    public event EventHandler<EventArgs>? EditEnded;

    protected void RaiseEditStarted()
    {
        EditStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void RaiseEditEnded()
    {
        EditEnded?.Invoke(this, EventArgs.Empty);
    }
}
