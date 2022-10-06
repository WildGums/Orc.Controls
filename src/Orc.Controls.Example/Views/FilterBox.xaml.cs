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

        private void OnFilterBoxControlInitializingAutoCompletionService(object sender, InitializingAutoCompletionServiceEventArgs e)
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable
            e.AutoCompletionService = this.GetTypeFactory().CreateInstance<ReverseAutoCompletionService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
        }
    }
}
