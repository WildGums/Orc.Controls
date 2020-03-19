// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftApiSelectDirectoryService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using Microsoft.WindowsAPICodePack.Dialogs;

    [ObsoleteEx(ReplacementTypeOrMember = "Orchestra.Services.MicrosoftApiSelectDirectoryService", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
    // NOTE: Don't forget to remove the references when removing this class
    public class MicrosoftApiSelectDirectoryService : ISelectDirectoryService
    {
        #region Properties
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string DirectoryName { get; private set; }
        public bool ShowNewFolderButton { get; set; }
        public string InitialDirectory { get; set; }
        public string Title { get; set; }
        #endregion

        #region ISelectDirectoryService Members
        public async Task<bool> DetermineDirectoryAsync()
        {
            var browserDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = Title,
                InitialDirectory = InitialDirectory
            };

            if (browserDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryName = browserDialog.FileName;
                return true;
            }

            DirectoryName = string.Empty;
            return false;
        }

        public async Task<DetermineDirectoryResult> DetermineDirectoryAsync(DetermineDirectoryContext context)
        {
            Argument.IsNotNull(() => context);

            var browserDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = Title,
                InitialDirectory = context.InitialDirectory
            };

            var dialogResult = browserDialog.ShowDialog();

            var result = new DetermineDirectoryResult
            {
                Result = dialogResult == CommonFileDialogResult.Ok,
            };

            // Note: only get properties when succeeded, otherwise it will throw exceptions
            if (result.Result)
            {
                result.DirectoryName = browserDialog.FileName;
            }

            return result;
        }
        #endregion
    }
}
