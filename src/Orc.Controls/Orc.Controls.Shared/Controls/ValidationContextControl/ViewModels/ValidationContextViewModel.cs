// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    public class ValidationContextViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;
        private readonly IValidationContext _injectedValidationContext;
        private readonly IDispatcherService _dispatcherService;

        public ValidationContextViewModel(IProcessService processService)
        {
            Argument.IsNotNull(() => processService);

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
            Argument.IsNotNull(() => dispatcherService);

            _injectedValidationContext = validationContext;
            _dispatcherService = dispatcherService;
        }

        public bool IsExpandedAllOnStartup { get; set; }
        public IValidationContext ValidationContext { get; set; }
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

        #region Commands
        public Command ExpandAll { get; }

        private void OnExpandAllExecute()
        {
            IsExpanded = true;
        }

        public Command CollapseAll { get; }

        private void OnCollapseAllExecute()
        {
            IsExpanded = false;
        }

        public Command Copy { get; }

        private bool OnCopyCanExecute()
        {
            return Nodes != null && Nodes.Any(x => x.IsVisible);
        }

        private void OnCopyExecute()
        {
            var text = Nodes.ToText();

            Clipboard.SetText(text);
        }

        public Command Open { get; }

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
            _processService.StartProcess(filePath);
        }
        #endregion

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
            if (Nodes == null)
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

            if (_injectedValidationContext != null)
            {
                _dispatcherService.BeginInvoke(() => ValidationContext = _injectedValidationContext);
            }
        }
    }
}