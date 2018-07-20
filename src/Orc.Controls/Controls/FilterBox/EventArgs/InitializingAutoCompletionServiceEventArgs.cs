// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Windows;
    using Catel.Services;

    /// <summary>
    /// The Initializing Auto Completion Service EventArgs
    /// </summary>
    public class InitializingAutoCompletionServiceEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Sets and gets the AutoCompletionService.
        /// </summary>
        public IAutoCompletionService AutoCompletionService { get; set; }
    }
}