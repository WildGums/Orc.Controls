namespace Orc.Controls
{
    using System.Windows;
    using Catel.Services;

    /// <summary>
    /// The Initializing Auto Completion Service EventArgs
    /// </summary>
    public class InitializingAutoCompletionServiceEventArgs : RoutedEventArgs
    {
        #region Properties
        /// <summary>
        /// Sets and gets the AutoCompletionService.
        /// </summary>
        public IAutoCompletionService AutoCompletionService { get; set; }
        #endregion
    }
}
