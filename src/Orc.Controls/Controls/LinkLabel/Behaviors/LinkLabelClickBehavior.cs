// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkLabelClickBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



#if NET

namespace Orc.Controls
{
    /// <summary>
    /// Available <see cref="LinkLabel"/> clickevent behaviors.
    /// </summary>
    public enum LinkLabelClickBehavior
    {
        /// <summary>
        /// No explicit behavior defined, will use the set-click-event.
        /// </summary>
        Undefined,

        /// <summary>
        /// Opens the set url in the systems webbrowser.
        /// </summary>
        OpenUrlInBrowser
    }
}

#endif