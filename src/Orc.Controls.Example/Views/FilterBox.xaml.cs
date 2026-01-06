namespace Orc.Controls.Example.Views
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public partial class FilterBox
    {
        private void OnFilterBoxControlInitializingAutoCompletionService(object sender, InitializingAutoCompletionServiceEventArgs e)
        {
            e.AutoCompletionService = ActivatorUtilities.CreateInstance<ReverseAutoCompletionService>(ServiceProvider);
        }
    }
}
