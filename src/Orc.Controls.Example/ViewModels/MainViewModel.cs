namespace Orc.Controls.Example.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;

        public MainViewModel(IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => uiVisualizerService);

            _uiVisualizerService = uiVisualizerService;

            DeferValidationUntilFirstSaveCall = false;

            StartTestHost = new TaskCommand(OnStartTestHostAsync);
        }

        public override string Title => "Orc.Controls example";

        public TaskCommand StartTestHost { get; set; }
        public TaskCommand StartRecord { get; set; }

        private async Task OnStartTestHostAsync()
        {
            await _uiVisualizerService.ShowDialogAsync<TestHostViewModel>();
        }
    }
}
