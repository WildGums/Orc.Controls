// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlToolManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools
{
    using System;
    using System.Collections.Generic;

    public interface IControlToolManager
    {
        #region Properties
        IList<IControlTool> Tools { get; }
        #endregion

        #region Methods
        object AttachTool(Type toolType);
        bool DetachTool(Type toolType);
        #endregion
    }
}
