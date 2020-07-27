// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextInputWindowService.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Catel.Services;
    using ViewModels;

    public class TextInputWindowService : ITextInputWindowService
    {
        #region Fields
        private readonly ITypeFactory _typeFactory;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public TextInputWindowService(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => uiVisualizerService);

            _typeFactory = typeFactory;
            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        #region ITextInputWindowService Members
        public async Task<TextInputDialogResult> ShowDialogAsync(string title, string initialText)
        {
            var viewModel = _typeFactory.CreateInstanceWithParametersAndAutoCompletion<TextInputViewModel>("Rename column");
            viewModel.Text = initialText;

            var dialogResult = await _uiVisualizerService.ShowDialogAsync(viewModel);

            return new TextInputDialogResult {Result = dialogResult, Text = viewModel.Text};
        }
        #endregion
    }
}
