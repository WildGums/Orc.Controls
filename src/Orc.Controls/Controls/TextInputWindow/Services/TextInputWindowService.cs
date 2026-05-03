namespace Orc.Controls.Services;

using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using ViewModels;

public class TextInputWindowService : ITextInputWindowService
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly IUIVisualizerService _uiVisualizerService;

    public TextInputWindowService(IViewModelFactory viewModelFactory, IUIVisualizerService uiVisualizerService)
    {
        _viewModelFactory = viewModelFactory;
        _uiVisualizerService = uiVisualizerService;
    }

    public async Task<TextInputDialogResult> ShowDialogAsync(string title, string initialText)
    {
        var viewModel = _viewModelFactory.CreateRequiredViewModel<TextInputViewModel>("Rename column");
        viewModel.Text = initialText;

        var dialogResult = await _uiVisualizerService.ShowDialogAsync(viewModel);

        return new TextInputDialogResult
        {
            Result = dialogResult.DialogResult,
            Text = viewModel.Text
        };
    }
}
