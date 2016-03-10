// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedValidationContextControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Windows;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;

    internal class AdvancedValidationContextControlViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;

        public AdvancedValidationContextControlViewModel(IProcessService processService)
        {
            Argument.IsNotNull(() => processService);

            _processService = processService;

            Copy = new Command(OnCopyExecute);
            Open = new Command(OnOpenExecute);
        }

        public ValidationContext ValidationContext { get; set; }
        public bool ShowErrors { get; set; } = true;
        public bool ShowWarnings { get; set; } = true;
        public int ErrorsCount { get; private set; }
        public int WarningsCount { get; private set; }
        public List<IValidationResult> ValidationResults { get; private set; }
        public bool ShowFilterBox { get; set; }
        public string Filter { get; set; }
        public IEnumerable<IValidationContextTreeNode> Nodes { get; set; }

        #region Commands
        public Command Copy { get; private set; }

        private bool OnCopyCanExecute()
        {
            return Nodes != null && Nodes.Any(x => x.IsVisible);
        }

        private void OnCopyExecute()
        {
            var text = Nodes.ToText();

            Clipboard.SetText(text);
        }

        public Command Open { get; private set; }

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
    }
}