// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    /// <summary>
    /// Load behavior of the tabs in the <see cref="TabControl"/>.
    /// </summary>
    public enum LoadTabItemsBehavior
    {
        /// <summary>
        /// Load all tabs using lazy loading, but keeps the tabs in memory afterwards.
        /// </summary>
        LazyLoading,

        /// <summary>
        /// Load all tabs using lazy loading. As soon as a tab is loaded, all other loaded tabs will be unloaded.
        /// </summary>
        LazyLoadingUnloadOthers,

        /// <summary>
        /// Load all tabs as soon as the tab control is loaded.
        /// </summary>
        EagerLoading,

        /// <summary>
        /// Load all tabs when any of the tabs is used for the first time.
        /// </summary>
        EagerLoadingOnFirstUse,
    }
}
