// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkLabelBehavior.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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