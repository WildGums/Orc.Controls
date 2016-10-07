// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRichTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class BindableRichTextBox : RichTextBox
    {
        #region Properties
        public static readonly DependencyProperty BindableDocumentProperty = DependencyProperty.Register("BindableDocument",
            typeof(FlowDocument), typeof(BindableRichTextBox), new PropertyMetadata(OnDocumentChanged));

        public FlowDocument BindableDocument
        {
            get { return (FlowDocument) GetValue(BindableDocumentProperty); }
            set { SetValue(BindableDocumentProperty, value); }
        }
        #endregion

        #region Methods
        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisControl = (RichTextBox) d;

            thisControl.Document = (e.NewValue == null) ? new FlowDocument() : (FlowDocument) e.NewValue;
        }
        #endregion        
    }
}