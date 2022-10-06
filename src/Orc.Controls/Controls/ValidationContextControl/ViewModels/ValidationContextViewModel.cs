namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    public class ValidationContextViewModel : ViewModelBase
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IValidationContext _injectedValidationContext;
        private readonly IProcessService _processService;

        public ValidationContextViewModel(IProcessService processService)
        {
            ArgumentNullException.ThrowIfNull(processService);

            _processService = processService;

            ExpandAll = new Command(OnExpandAllExecute);
            CollapseAll = new Command(OnCollapseAllExecute);
            Copy = new Command(OnCopyExecute, OnCopyCanExecute);
            Open = new Command(OnOpenExecute);

            InvalidateCommandsOnPropertyChanged = true;
        }

        public ValidationContextViewModel(ValidationContext validationContext, IProcessService processService, IDispatcherService dispatcherService)
            : this(processService)
        {
            ArgumentNullException.ThrowIfNull(dispatcherService);

            _injectedValidationContext = validationContext;
            _dispatcherService = dispatcherService;
        }

        public bool IsExpandedAllOnStartup { get; set; }
        public IValidationContext? ValidationContext { get; set; }
        public bool ShowErrors { get; set; } = true;
        public bool ShowWarnings { get; set; } = true;
        public int ErrorsCount { get; private set; }
        public int WarningsCount { get; private set; }
        public List<IValidationResult> ValidationResults { get; private set; }
        public bool ShowFilterBox { get; set; }
        public string Filter { get; set; }
        public IEnumerable<IValidationContextTreeNode> Nodes { get; set; }

        public bool IsExpanded { get; private set; }
        public bool IsCollapsed => !IsExpanded;

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
            return Nodes is not null && Nodes.Any(x => x.IsVisible);
        }

        private void OnCopyExecute()
        {
            var text = Nodes.ToText();

            Clipboard.SetText(text);
        }

        private void OnOpenExecute()
        {
            var path = string.Empty;

            try
            {
                path = Path.GetTempPath();
            }
            catch (SecurityException)
            {
                return;
            }

            var filePath = CreateValidationContextFile(path);
            _processService.StartProcess(new ProcessContext
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        private void OnNodesChanged()
        {
            UpdateNodesExpandedingState();
        }

        private void OnIsExpandedAllOnStartupChanged()
        {
            IsExpanded = IsExpandedAllOnStartup;
        }

        private void OnIsExpandedChanged()
        {
            UpdateNodesExpandedingState();
        }

        private void UpdateNodesExpandedingState()
        {
            if (Nodes is null)
            {
                return;
            }

            if (IsExpanded)
            {
                Nodes.ExpandAll();
            }
            else
            {
                Nodes.CollapseAll();
            }
        }

        private string CreateValidationContextFile(string path)
        {
            var filePath = Path.Combine(path, "ValidationContext.txt");
            File.WriteAllText(filePath, Nodes.ToText());
            return filePath;
        }

        private void OnValidationContextChanged()
        {
            var validationContext = ValidationContext;
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
}
