// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBox.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.Views
{
    using Catel.IoC;
    using Services;
    using Orc.Controls;

    public partial class FilterBox
    {
        public FilterBox()
        {
            InitializeComponent();
        }

        private void OnFilterBoxControlInitializingAutoCompletionService(object sender, Orc.Controls.InitializingAutoCompletionServiceEventArgs e)
        {
            e.AutoCompletionService = this.GetTypeFactory().CreateInstance<ReverseAutoCompletionService>();
        }
    }
}
