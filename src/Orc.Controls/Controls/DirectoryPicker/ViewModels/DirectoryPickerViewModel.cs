namespace Orc.Controls
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using FileSystem;

    public class DirectoryPickerViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private readonly IDirectoryService _directoryService;

        public DirectoryPickerViewModel(ISelectDirectoryService selectDirectoryService, IDirectoryService directoryService, IProcessService processService)
        {
            ArgumentNullException.ThrowIfNull(selectDirectoryService);
            ArgumentNullException.ThrowIfNull(directoryService);
            ArgumentNullException.ThrowIfNull(processService);

            _selectDirectoryService = selectDirectoryService;
            _directoryService = directoryService;
            _processService = processService;

            OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
            SelectDirectory = new TaskCommand(OnSelectDirectoryExecuteAsync);
            Clear = new Command(OnClearExecute, OnClearCanExecute);
        }

        public double LabelWidth { get; set; }

        public string? LabelText { get; set; }

        public string? SelectedDirectory { get; set; }

        public Command Clear { get; }

        private bool OnClearCanExecute()
        {
            return OnOpenDirectoryCanExecute();
        }

        private void OnClearExecute()
        {
            SelectedDirectory = string.Empty;
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
            return !string.IsNullOrWhiteSpace(SelectedDirectory);
        }

        /// <summary>
        /// Method to invoke when the OpenDirectory command is executed.
        /// </summary>
        private void OnOpenDirectoryExecute()
        {
            if (_directoryService.Exists(SelectedDirectory))
            {
                var fullPath = Path.GetFullPath(SelectedDirectory);
                _processService.StartProcess(new ProcessContext
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });
            }
        }

        /// <summary>
        /// Gets the SelectDirectory command.
        /// </summary>
        public TaskCommand SelectDirectory { get; private set; }

        /// <summary>
        /// Method to invoke when the SelectOutputDirectory command is executed.
        /// </summary>
        private async Task OnSelectDirectoryExecuteAsync()
        {
            string initialDirectory = null;

            if (!string.IsNullOrEmpty(SelectedDirectory))
            {
                initialDirectory = Path.GetFullPath(SelectedDirectory);
            }

            var result = await _selectDirectoryService.DetermineDirectoryAsync(new DetermineDirectoryContext
            {
                InitialDirectory = initialDirectory
            });

            if (result.Result)
            {
                SelectedDirectory = result.DirectoryName;
            }
        }
    }
}
