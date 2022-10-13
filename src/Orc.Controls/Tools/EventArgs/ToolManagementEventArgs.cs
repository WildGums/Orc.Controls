namespace Orc.Controls.Tools
{
    using System;

    public class ToolManagementEventArgs : EventArgs
    {
        public ToolManagementEventArgs(IControlTool tool)
        {
            ArgumentNullException.ThrowIfNull(tool);

            Tool = tool;
        }

        public IControlTool Tool { get; }
    }
}
