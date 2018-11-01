// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRichTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class BindableRichTextBox : RichTextBox
    {
        public BindableRichTextBox()
        {
            TextChanged += OnTextChanged;
        }

        #region Properties
        public FlowDocument BindableDocument
        {
            get { return (FlowDocument) GetValue(BindableDocumentProperty); }
            set { SetValue(BindableDocumentProperty, value); }
        }

        public static readonly DependencyProperty BindableDocumentProperty = DependencyProperty.Register(nameof(BindableDocument),
            typeof(FlowDocument), typeof(BindableRichTextBox),  new PropertyMetadata((sender, args) => ((BindableRichTextBox)sender).OnBindableDocumentChanged(args)));


        public bool AutoScrollToEnd
        {
            get { return (bool)GetValue(AutoScrollToEndProperty); }
            set { SetValue(AutoScrollToEndProperty, value); }
        }

        public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.Register(nameof(AutoScrollToEnd),
             typeof(bool), typeof(BindableRichTextBox), new PropertyMetadata(default(bool)));
        #endregion

        #region Methods
        private void OnBindableDocumentChanged(DependencyPropertyChangedEventArgs args)
        {
            Document = (args.NewValue == null) ? new FlowDocument() : (FlowDocument) args.NewValue;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoScrollToEnd)
            {
                ScrollToEnd();
            }
        }
        #endregion
    }
}
