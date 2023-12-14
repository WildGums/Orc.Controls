namespace Orc.Controls;

using System.Windows.Controls;
using System.Windows.Input;
using Catel.Windows.Interactivity;

public class DoNotShowDropDownOnClickComboboxBehavior : BehaviorBase<ComboBox>
{
    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        var combobox = AssociatedObject;

        combobox.PreviewMouseDown += OnPreviewMouseDown;
        combobox.PreviewMouseDoubleClick += OnPreviewMouseDoubleClick;
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        var combobox = AssociatedObject;

        combobox.PreviewMouseDown -= OnPreviewMouseDown;
        combobox.PreviewMouseDoubleClick -= OnPreviewMouseDoubleClick;

        base.OnAssociatedObjectUnloaded();
    }

    private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
    {
        var originalSource = args.OriginalSource;
        if (originalSource is not Border)
        {
            return;
        }

        Keyboard.Focus(AssociatedObject);
        args.Handled = true;
    }

    private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs args)
    {
        var originalSource = args.OriginalSource;
        if (originalSource is not Border)
        {
            return;
        }

        AssociatedObject.SetCurrentValue(ComboBox.IsDropDownOpenProperty, true);
        args.Handled = true;
    }
}
