namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using FileSystem;

public class ValidationContextViewModel : ViewModelBase
{
    private readonly IDispatcherService _dispatcherService;
    private readonly IFileService _fileService;
    private readonly IProcessService _processService;
    private readonly IValidationContext? _injectedValidationContext;

    public ValidationContextViewModel(IProcessService processService, IDispatcherService dispatcherService,
        IFileService fileService)
    {
        ArgumentNullException.ThrowIfNull(processService);
        ArgumentNullException.ThrowIfNull(dispatcherService);
        ArgumentNullException.ThrowIfNull(fileService);

        _processService = processService;
        _dispatcherService = dispatcherService;
        _fileService = fileService;

        ValidateUsingDataAnnotations = false;

        ExpandAll = new Command(OnExpandAllExecute);
        CollapseAll = new Command(OnCollapseAllExecute);
        Copy = new Command(OnCopyExecute, OnCopyCanExecute);
        Open = new Command(OnOpenExecute);
        
        Nodes = Enumerable.Empty<IValidationContextTreeNode>();

        InvalidateCommandsOnPropertyChanged = true;
    }

    public ValidationContextViewModel(ValidationContext validationContext, IProcessService processService, 
        IDispatcherService dispatcherService, IFileService fileService)
        : this(processService, dispatcherService, fileService)
    {
        _injectedValidationContext = validationContext;
    }

    public bool IsExpandedAllOnStartup { get; set; }
    public bool ShowErrors { get; set; } = true;
    public bool ShowWarnings { get; set; } = true; 
    public bool ShowFilterBox { get; set; }
    public bool IsExpanded { get; private set; }
    public bool IsCollapsed => !IsExpanded;
    public int ErrorsCount { get; private set; }
    public int WarningsCount { get; private set; }
    public string? Filter { get; set; }
    public IValidationContext? ValidationContext { get; set; }
    public List<IValidationResult>? ValidationResults { get; private set; }
    public IEnumerable<IValidationContextTreeNode>? Nodes { get; set; }
    
    public Command ExpandAll { get; }
    public Command CollapseAll { get; }
    public Command Copy { get; }
    public Command Open { get; }

    private void OnExpandAllExecute()
    {
        IsExpanded = true;
    }

    private void OnCollapseAllExecute()
    {
        IsExpanded = false;
    }

    private bool OnCopyCanExecute()
    {
        return Nodes?.Any(x => x.IsVisible) == true;
    }

    private void OnCopyExecute()
    {
        var nodes = Nodes;
        if (nodes is null)
        {
            return;
        }

        var text = nodes.ToText();

        Clipboard.SetText(text);
    }

    private void OnOpenExecute()
    {
        string path;

        try
        {
            path = Path.GetTempPath();
        }
        catch (SecurityException)
        {
            return;
        }

        if (!TryCreateValidationContextFile(path, out var filePath))
        {
            return;
        }

        _processService.StartProcess(new ProcessContext
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }

    private void OnNodesChanged()
    {
        UpdateNodesExpandedState();
    }

    private void OnIsExpandedAllOnStartupChanged()
    {
        IsExpanded = IsExpandedAllOnStartup;
    }

    private void OnIsExpandedChanged()
    {
        UpdateNodesExpandedState();
    }

    private void UpdateNodesExpandedState()
    {
        var nodes = Nodes;
        if (nodes is null)
        {
            return;
        }

        if (!nodes.Any())
        {
            return;
        }

        if (IsExpanded)
        {
            nodes.ExpandAll();
        }
        else
        {
            nodes.CollapseAll();
        }
    }

    private bool TryCreateValidationContextFile(string path, [NotNullWhen(true)] out string? filePath)
    {
        filePath = null;

        var nodes = Nodes;
        if (nodes is null)
        {
            return false;
        }

        filePath = Path.Combine(path, "ValidationContext.txt");
        _fileService.WriteAllText(filePath, nodes.ToText());
        return true;
    }

    private void OnValidationContextChanged()
    {
        var validationContext = ValidationContext;

        if (validationContext is null)
        {
            ErrorsCount = 0;
            WarningsCount = 0;
            ValidationResults = new List<IValidationResult>();

            return;
        }

        ErrorsCount = validationContext.GetErrorCount();
        WarningsCount = validationContext.GetWarningCount();
        ValidationResults = validationContext.GetValidations();
    }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        if (_injectedValidationContext is not null)
        {
            _dispatcherService.BeginInvoke(() => ValidationContext = _injectedValidationContext);
        }
    }
}
