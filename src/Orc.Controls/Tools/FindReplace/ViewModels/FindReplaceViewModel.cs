// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System.Media;
    using Catel;
    using Catel.MVVM;
    using Services;

    public class FindReplaceViewModel : ViewModelBase
    {
        #region Fields
        private readonly IFindReplaceService _findReplaceService;
        #endregion

        #region Constructors
        public FindReplaceViewModel(IFindReplaceService findReplaceService)
        {
            Argument.IsNotNull(() => findReplaceService);

            _findReplaceService = findReplaceService;

            FindNext = new Command<string>(OnFindNext);
            Replace = new Command<object>(OnReplace);
            ReplaceAll = new Command<object>(OnReplaceAll);

            FindReplaceSettings = new FindReplaceSettings();

            var initialText = _findReplaceService.GetInitialFindText();

            TextToFind = initialText;
            TextToFindForReplace = initialText;
        }
        #endregion

        #region Properties
        public override string Title => "Find and Replace";

        [Model]
        public FindReplaceSettings FindReplaceSettings { get; }

        public string TextToFind { get; set; }
        public string TextToFindForReplace { get; set; }
        public Command<string> FindNext { get; }
        public Command<object> Replace { get; }
        public Command<object> ReplaceAll { get; }
        #endregion

        #region Methods
        private void OnReplaceAll(object parameter)
        {
            var values = (object[])parameter;
            var textToFind = values[0] as string ?? string.Empty;
            var replacementText = values[1] as string ?? string.Empty;

            _findReplaceService.ReplaceAll(textToFind, replacementText, FindReplaceSettings);
        }

        private void OnReplace(object parameter)
        {
            var values = (object[])parameter;
            var textToFind = values[0] as string ?? string.Empty;
            var replacementText = values[1] as string ?? string.Empty;

            if (!_findReplaceService.Replace(textToFind, replacementText, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }
        }

        private void OnFindNext(string text)
        {
            var textToFind = text ?? string.Empty;

            if (!_findReplaceService.FindNext(textToFind, FindReplaceSettings))
            {
                SystemSounds.Beep.Play();
            }
        }
        #endregion
    }
}
