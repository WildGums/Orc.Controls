namespace Orc.Controls.Services;

using System;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.Services;
using ViewModels;

public class TextInputWindowService : ITextInputWindowService
{
    private readonly ITypeFactory _typeFactory;
    private readonly IUIVisualizerService _uiVisualizerService;

    public TextInputWindowService(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
    {
        ArgumentNullException.ThrowIfNull(typeFactory);
        ArgumentNullException.ThrowIfNull(uiVisualizerService);

        _typeFactory = typeFactory;
        _uiVisualizerService = uiVisualizerService;
    }

    public async Task<TextInputDialogResult> ShowDialogAsync(string title, string initialText)
    {
        var viewModel = _typeFactory.CreateRequiredInstanceWithParametersAndAutoCompletion<TextInputViewModel>("Rename column");
        viewModel.Text = initialText;

        var dialogResult = await _uiVisualizerService.ShowDialogAsync(viewModel);

        return new TextInputDialogResult
        {
            Result = dialogResult.DialogResult,
            Text = viewModel.Text
        };
    }
}