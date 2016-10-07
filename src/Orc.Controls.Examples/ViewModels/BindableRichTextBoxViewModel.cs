// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRichTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel.MVVM;

    public class BindableRichTextBoxViewModel : ViewModelBase
    {
        #region Constructors
        public BindableRichTextBoxViewModel()
        {
            AccentColorBrush = Brushes.Orange;
            FlowDoc = new FlowDocument();
            FlowDoc.Foreground = AccentColorBrush.Clone();
        }
        #endregion

        #region Properties
        public FlowDocument FlowDoc { get; set; }
        public Brush AccentColorBrush { get; set; }
        #endregion
    }
}