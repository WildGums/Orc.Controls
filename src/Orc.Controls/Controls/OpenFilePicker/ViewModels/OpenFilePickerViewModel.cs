namespace Orc.Controls;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using Path = Catel.IO.Path;

public class OpenFilePickerViewModel : ViewModelBase
{
    private readonly IProcessService _processService;
    private readonly IOpenFileService _openFileService;

    public OpenFilePickerViewModel(IOpenFileService openFileService, IProcessService processService)
    {
        ArgumentNullException.ThrowIfNull(openFileService);
        ArgumentNullException.ThrowIfNull(processService);

        _openFileService = openFileService;
        _processService = processService;

        ValidateUsingDataAnnotations = false;

        OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
        SelectFile = new TaskCommand(OnSelectFileExecuteAsync);
        Clear = new Command(OnClearExecute, OnClearCanExecute);
    }

    public double LabelWidth { get; set; }

    public string? LabelText { get; set; }

    public string? Filter { get; set; }

    public string? SelectedFile { get; set; }

    public string? BaseDirectory { get; set; }

    public string? SelectedFileDisplayName { get; private set; }

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
    public Command OpenDirectory { get; }

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
        if (!string.IsNullOrWhiteSpace(initialDirectory))
        {
            _processService.StartProcess(new ProcessContext
            {
                FileName = initialDirectory,
                UseShellExecute = true
            });
        }
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
        string? initialDirectory = null;
        string? fileName = null;
        string? filter = null;

        if (!string.IsNullOrEmpty(SelectedFile))
        {
            initialDirectory = Directory.GetParent(SelectedFile)?.FullName;
            fileName = SelectedFile;
        }

        if (!string.IsNullOrEmpty(Filter))
        {
            filter = Filter;
        }

        var result = await _openFileService.DetermineFileAsync(new DetermineOpenFileContext
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
        if (string.IsNullOrWhiteSpace(selectedFile) || !System.IO.Path.IsPathRooted(selectedFile))
        {
            SelectedFileDisplayName = string.Empty;
            return;
        }

        var baseDirectory = BaseDirectory;
        if (string.IsNullOrWhiteSpace(baseDirectory) || !Directory.Exists(baseDirectory))
        {
            SelectedFileDisplayName = selectedFile;
            return;
        }

        var selectedFileRoot = System.IO.Path.GetPathRoot(selectedFile);
        var baseDirectoryRoot = System.IO.Path.GetPathRoot(baseDirectory);
        if (!string.Equals(selectedFileRoot, baseDirectoryRoot, StringComparison.OrdinalIgnoreCase))
        {
            SelectedFileDisplayName = selectedFile;
            return;
        }

        SelectedFileDisplayName = Path.GetRelativePath(selectedFile, baseDirectory);
    }

    private string GetInitialDirectory()
    {
        var selectedFile = SelectedFile;
        if (string.IsNullOrWhiteSpace(selectedFile))
        {
            return string.Empty;
        }

        var isSelectedRootedPath = System.IO.Path.IsPathRooted(selectedFile);

        string? result = null;
        if (isSelectedRootedPath)
        {
            result = Path.GetParentDirectory(selectedFile);
        }

        if (!string.IsNullOrWhiteSpace(result) && Directory.Exists(result))
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
