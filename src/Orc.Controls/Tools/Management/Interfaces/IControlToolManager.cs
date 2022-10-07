namespace Orc.Controls.Tools
{
    using System;
    using System.Collections.Generic;

    public interface IControlToolManager
    {
        IList<IControlTool> Tools { get; }

        bool CanAttachTool(Type toolType);
        object AttachTool(Type toolType);
        bool DetachTool(Type toolType);

        event EventHandler<ToolManagementEventArgs>? ToolAttached;
        event EventHandler<ToolManagementEventArgs>? ToolDetached;
        event EventHandler<ToolManagementEventArgs>? ToolOpening;
        event EventHandler<ToolManagementEventArgs>? ToolOpened;
        event EventHandler<ToolManagementEventArgs>? ToolClosed;
    }
}
