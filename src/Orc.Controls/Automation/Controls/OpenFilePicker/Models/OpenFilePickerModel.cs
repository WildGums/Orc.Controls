namespace Orc.Controls.Automation;

using Orc.Automation;

[ActiveAutomationModel]
public class OpenFilePickerModel : FrameworkElementModel
{
    public OpenFilePickerModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }
    
    public double LabelWidth { get; set; }
    public string LabelText { get; set; }
    public string SelectedFile { get; set; }
    public string Filter { get; set; }
    public string BaseDirectory { get; set; }
}
