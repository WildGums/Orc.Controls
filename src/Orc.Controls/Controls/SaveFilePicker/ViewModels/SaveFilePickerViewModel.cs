namespace Orc.Controls;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;

public class SaveFilePickerViewModel : ViewModelBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly IProcessService _processService;
    private readonly ISaveFileService _saveFileService;

    public SaveFilePickerViewModel(ISaveFileService saveFileService, IProcessService processService)
    {
        ArgumentNullException.ThrowIfNull(saveFileService);
        ArgumentNullException.ThrowIfNull(processService);

        _saveFileService = saveFileService;
        _processService = processService;

        OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
        SelectFile = new TaskCommand(OnSelectFileExecuteAsync);
        Clear = new Command(OnClearExecute, OnClearCanExecute);
    }

    public double LabelWidth { get; set; }

    public string? LabelText { get; set; }

    public string? SelectedFile { get; set; }

    public string? InitialDirectory { get; set; }

    public string? InitialFileName { get; set; }

    public string? Filter { get; set; }

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
    public Command OpenDirectory { get; }

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
        return directory is not null &&
               // Don't allow users to write text that they can "invoke" via our software
               directory.Exists;
    }

    /// <summary>
    /// Method to invoke when the OpenDirectory command is executed.
    /// </summary>
    private void OnOpenDirectoryExecute()
    {
        var selectedFile = SelectedFile;
        if (string.IsNullOrWhiteSpace(selectedFile))
        {
            Log.Warning("Can't open directory, because selected file isn't selected");

            return;
        }

        var directory = Directory.GetParent(selectedFile);
        if (directory is null)
        {
            Log.Warning($"Can't find parent directory for selected file: '{selectedFile}'");

            return;
        }

        _processService.StartProcess(new ProcessContext
        {
            FileName = directory.FullName,
            UseShellExecute = true
        });
    }

    /// <summary>
    /// Gets the SelectFile command.
    /// </summary>
    public TaskCommand SelectFile { get; }

    /// <summary>
    /// Method to invoke when the SelectFile command is executed.
    /// </summary>
    private async Task OnSelectFileExecuteAsync()
    {
        string? initialDirectory;
        string? fileName;
        string? filter = null;

        if (!string.IsNullOrEmpty(SelectedFile))
        {
            initialDirectory = Directory.GetParent(SelectedFile)?.FullName;
            fileName = SelectedFile;
        }
        else
        {
            initialDirectory = InitialDirectory;
            fileName = InitialFileName;
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
}
