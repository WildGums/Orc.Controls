// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFilePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.IO;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Path = Catel.IO.Path;

    public class OpenFilePickerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProcessService _processService;
        private readonly IOpenFileService _selectFileService;
        #endregion

        #region Constructors
        public OpenFilePickerViewModel(IOpenFileService selectFileService, IProcessService processService)
        {
            Argument.IsNotNull(() => selectFileService);
            Argument.IsNotNull(() => processService);

            _selectFileService = selectFileService;
            _processService = processService;

            OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
            SelectFile = new Command(OnSelectFileExecute);
        }
        #endregion

        #region Properties
        public double LabelWidth { get; set; }

        public string LabelText { get; set; }

        public string Filter { get; set; }

        public string SelectedFile { get; set; }

        public string BaseDirectory { get; set; }

        public string SelectedFileDisplayName { get; private set; }
        #endregion

        #region Commands
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
            var initialDirectory = GetInitialDirectory();
            return !string.IsNullOrWhiteSpace(initialDirectory);
        }

        /// <summary>
        /// Method to invoke when the OpenDirectory command is executed.
        /// </summary>
        private void OnOpenDirectoryExecute()
        {
            var initialDirectory = GetInitialDirectory();
            if(!string.IsNullOrWhiteSpace(initialDirectory))
            {
                _processService.StartProcess(initialDirectory);
            }
        }

        /// <summary>
        /// Gets the SelectFile command.
        /// </summary>
        public Command SelectFile { get; private set; }

        /// <summary>
        /// Method to invoke when the SelectFile command is executed.
        /// </summary>
        private void OnSelectFileExecute()
        {
            var initialDirectory = GetInitialDirectory();
            if (!string.IsNullOrEmpty(initialDirectory))
            {
                _selectFileService.InitialDirectory = initialDirectory;
            }

            if (!string.IsNullOrEmpty(Filter))
            {
                _selectFileService.Filter = Filter;
            }

            if (_selectFileService.DetermineFile())
            {
                SelectedFile = _selectFileService.FileName;
            }
        }
        #endregion

        private void OnBaseDirectoryChanged()
        {
            UpdateSelectedFileDisplayName();
        }

        private void OnSelectedFileChanged()
        {
            UpdateSelectedFileDisplayName();
        }

        private void UpdateSelectedFileDisplayName()
        {
            var selectedFile = SelectedFile;
            if(string.IsNullOrWhiteSpace(selectedFile) || !System.IO.Path.IsPathRooted(selectedFile))
            {
                SelectedFileDisplayName = string.Empty;
                return;
            }

            var baseDirectory = BaseDirectory;
            if(string.IsNullOrWhiteSpace(baseDirectory) || !Directory.Exists(baseDirectory))
            {
                SelectedFileDisplayName = selectedFile;
                return;
            }

            var selectedFileRoot = System.IO.Path.GetPathRoot(selectedFile);
            var baseDirectoryRoot = System.IO.Path.GetPathRoot(baseDirectory);
            if(!string.Equals(selectedFileRoot, baseDirectoryRoot, StringComparison.OrdinalIgnoreCase))
            {
                SelectedFileDisplayName = selectedFile;
                return;
            }

            SelectedFileDisplayName = Path.GetRelativePath(selectedFile, baseDirectory);
        }

        private string GetInitialDirectory()
        {
            var selectedFile = SelectedFile;
            var isSelectedRootedPath = !string.IsNullOrWhiteSpace(selectedFile) && System.IO.Path.IsPathRooted(selectedFile);

            string result = null;
            if(isSelectedRootedPath)
            {
                result = Path.GetParentDirectory(selectedFile);
            }

            if(!string.IsNullOrWhiteSpace(result) && Directory.Exists(result))
            {
                return result;
            }

            result = BaseDirectory;
            if (!string.IsNullOrWhiteSpace(result) && Directory.Exists(result))
            {
                return result;
            }

            return string.Empty;
        }
    }
}