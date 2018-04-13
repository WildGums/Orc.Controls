// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlAdornerChild.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;

    /// <summary>
    /// The ControlAdornerChild interface.
    /// </summary>
    internal interface IControlAdornerChild
    {
        /// <summary>
        /// The get position.
        /// </summary>
        /// <returns>The <see cref="Point" />.</returns>
        Point GetPosition();
    }
}