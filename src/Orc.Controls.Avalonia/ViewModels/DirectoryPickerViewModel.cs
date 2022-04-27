namespace Orc.Controls.Avalonia.ViewModels
{
    using System.IO;
    using System.Reactive;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using Orc.FileSystem;
    using ReactiveUI;

    public class DirectoryPickerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProcessService _processService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private readonly IDirectoryService _directoryService;

        private string _directory;
        #endregion

        #region Constructors
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DirectoryPickerViewModel(ISelectDirectoryService selectDirectoryService, IDirectoryService directoryService, IProcessService processService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => processService);

            _selectDirectoryService = selectDirectoryService;
            _directoryService = directoryService;
            _processService = processService;

            OpenDirectory = ReactiveCommand.Create(OnOpenDirectoryExecute);
            SelectDirectory = ReactiveCommand.Create(OnSelectDirectoryExecuteAsync);
            Clear = ReactiveCommand.Create(OnClearExecute);
        }
        #endregion

        #region Properties
        public double LabelWidth { get; set; }

        public string LabelText { get; set; }

        public string SelectedDirectory { get => _directory; set => this.RaiseAndSetIfChanged(ref _directory, value); }

        #endregion

        #region Commands
#pragma warning disable IDISP006 // Implement IDisposable
        public ReactiveCommand<Unit, Unit> Clear { get; }
#pragma warning restore IDISP006 // Implement IDisposable

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
#pragma warning disable IDISP006 // Implement IDisposable
        public ReactiveCommand<Unit, Unit> OpenDirectory { get; private set; }
#pragma warning restore IDISP006 // Implement IDisposable

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
#pragma warning disable IDISP006 // Implement IDisposable
        public ReactiveCommand<Unit, Task> SelectDirectory { get; private set; }
#pragma warning restore IDISP006 // Implement IDisposable

        /// <summary>
        /// Method to invoke when the SelectOutputDirectory command is executed.
        /// </summary>
        private async Task OnSelectDirectoryExecuteAsync()
        {
            string? initialDirectory = null;

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
        #endregion
    }
}
