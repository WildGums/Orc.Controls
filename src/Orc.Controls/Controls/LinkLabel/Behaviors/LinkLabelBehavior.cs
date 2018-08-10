// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkLabelBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


#if NET

namespace Orc.Controls
{
    /// <summary>
    /// Available <see cref="LinkLabel"/> behaviors.
    /// </summary>
    public enum LinkLabelBehavior
    {
        /// <summary>
        /// Default.
        /// </summary>
        SystemDefault,

        /// <summary>
        /// Always underline.
        /// </summary>
        AlwaysUnderline,

        /// <summary>
        /// Hover underline.
        /// </summary>
        HoverUnderline,

        /// <summary>
        /// Never underline.
        /// </summary>
        NeverUnderline
    }
}

#endif
