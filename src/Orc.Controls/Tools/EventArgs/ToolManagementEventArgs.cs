// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolManagementEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools
{
    using System;
    using Catel;

    public class ToolManagementEventArgs : EventArgs
    {
        #region Constructors
        public ToolManagementEventArgs(IControlTool tool)
        {
            ArgumentNullException.ThrowIfNull(tool);

            Tool = tool;
        }
        #endregion

        #region Properties
        public IControlTool Tool { get; }
        #endregion
    }
}
