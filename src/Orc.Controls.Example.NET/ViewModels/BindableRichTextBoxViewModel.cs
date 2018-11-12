// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRichTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
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

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public Brush AccentColorBrush { get; set; }
        #endregion
    }
}
