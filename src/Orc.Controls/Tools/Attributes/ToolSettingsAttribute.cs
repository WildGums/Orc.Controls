// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToolSettingsAttribute.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Tools.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ToolSettingsAttribute : Attribute
    {
        #region Properties
        public string Storage { get; set; }
        #endregion
    }
}
