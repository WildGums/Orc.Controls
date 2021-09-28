// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFilePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class SaveFilePickerViewModel : ViewModelBase
    {
        #region Constructors
        public SaveFilePickerViewModel(ISaveFileService saveFileService, IProcessService processService)
        {
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => processService);

            _saveFileService = saveFileService;
            _processService = processService;

            OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
            SelectFile = new TaskCommand(OnSelectFileExecuteAsync);
            Clear = new Command(OnClearExecute, OnClearCanExecute);
        }
        #endregion

        #region Fields
        private readonly IProcessService _processService;
        private readonly ISaveFileService _saveFileService;
        #endregion

        #region Properties
        public double LabelWidth { get; set; }

        public string LabelText { get; set; }

        public string SelectedFile { get; set; }

        public string Filter { get; set; }
        #endregion

        #region Commands
        public Command Clear { get; }

        private bool OnClearCanExecute()
        {
            return OnOpenDirectoryCanExecute();
        }

        private void OnClearExecute()
        {
            SelectedFile = string.Empty;
        }

        /// <summary>
        /// Gets the OpenDirectory command.
        /// </summary>
        public Command OpenDirectory { get; private set; }

        /// <summary>
        /// Method to check whether the OpenDirectory command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnOpenDirectoryCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SelectedFile))
            {
                return false;
            }

            var directory = Directory.GetParent(SelectedFile);
            // Don't allow users to write text that they can "invoke" via our software
            return directory.Exists;
        }

        /// <summary>
        /// Method to invoke when the OpenDirectory command is executed.
        /// </summary>
        private void OnOpenDirectoryExecute()
        {
            var directory = Directory.GetParent(SelectedFile);
            _processService.StartProcess(new ProcessContext
            {
                FileName = directory.FullName,
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Gets the SelectFile command.
        /// </summary>
        public TaskCommand SelectFile { get; private set; }

        /// <summary>
        /// Method to invoke when the SelectFile command is executed.
        /// </summary>
        private async Task OnSelectFileExecuteAsync()
        {
            string initialDirectory = null;
            string fileName = null;
            string filter = null;

            if (!string.IsNullOrEmpty(SelectedFile))
            {
                initialDirectory = Directory.GetParent(SelectedFile).FullName;
                fileName = SelectedFile;
            }

            if (!string.IsNullOrEmpty(Filter))
            {
                filter = Filter;
            }

            var result = await _saveFileService.DetermineFileAsync(new DetermineSaveFileContext
            {
                InitialDirectory = initialDirectory,
                FileName = fileName,
                Filter = filter,
            });

            if (result.Result)
            {
                var oldSelectedFile = SelectedFile;

                SelectedFile = result.FileName;

                // See here: https://github.com/WildGums/Orc.Controls/issues/13
                if (!AlwaysInvokeNotifyChanged
                    && string.Equals(oldSelectedFile, SelectedFile))
                {
                    RaisePropertyChanged(nameof(SelectedFile));
                }
            }
        }
        #endregion
    }
}
