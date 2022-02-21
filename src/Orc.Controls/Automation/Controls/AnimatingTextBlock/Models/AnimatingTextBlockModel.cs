namespace Orc.Controls.Automation;

using Orc.Automation;

[AutomationAccessType]
public class AnimatingTextBlockModel : FrameworkElementModel
{
    public AnimatingTextBlockModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string Text { get; set; }
}
