namespace Orc.Controls.Tools;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IControlToolManager
{
    IList<IControlTool> Tools { get; }

    bool CanAttachTool(Type toolType);
    Task<object?> AttachToolAsync(Type toolType);
    Task<bool> DetachToolAsync(Type toolType);

    event EventHandler<ToolManagementEventArgs>? ToolAttached;
    event EventHandler<ToolManagementEventArgs>? ToolDetached;
    event EventHandler<ToolManagementEventArgs>? ToolOpening;
    event EventHandler<ToolManagementEventArgs>? ToolOpened;
    event EventHandler<ToolManagementEventArgs>? ToolClosed;
}
