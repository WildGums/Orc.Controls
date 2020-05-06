// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBox.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.Views
{
    using Catel.IoC;
    using Services;

    public partial class FilterBox
    {
        public FilterBox()
        {
            InitializeComponent();
        }

        private void OnFilterBoxControlInitializingAutoCompletionService(object sender, Controls.InitializingAutoCompletionServiceEventArgs e)
        {
            e.AutoCompletionService = this.GetTypeFactory().CreateInstance<ReverseAutoCompletionService>();
        }
    }
}