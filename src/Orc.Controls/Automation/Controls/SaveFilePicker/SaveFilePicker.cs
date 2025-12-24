namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.SaveFilePicker))]
public class SaveFilePicker : FrameworkElement<SaveFilePickerModel, SaveFilePickerMap>
{
    //private AutomationElement _saveDialog;

    public SaveFilePicker(AutomationElement element)
        : base(element)
    {
    }
}
