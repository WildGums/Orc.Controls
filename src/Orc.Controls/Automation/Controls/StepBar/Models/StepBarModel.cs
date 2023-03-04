﻿#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Windows.Controls;
using Orc.Automation;

public class StepBarModel : FrameworkElementModel
{
    public StepBarModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    [ActiveAutomationProperty]
    public Orientation Orientation { get; set; }

    [ActiveAutomationProperty]
    public List<IStepBarItem> Items { get; set; }

    public IStepBarItem SelectedItem
    {
        get => _accessor.GetValue<IStepBarItem>();
        set
        {
            if (value is null)
            {
                return;
            }

            _accessor.ExecuteAutomationMethod<StepBarSelectItemByTitleNumberDescriptionMethodRun>(value.Title, value.Number, value.Description);
        }
    }
}
#nullable enable
