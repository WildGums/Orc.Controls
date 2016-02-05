// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MicrosoftApiSelectDirectoryService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using Catel.Services;
    using Microsoft.WindowsAPICodePack.Dialogs;

    public class MicrosoftApiSelectDirectoryService : ISelectDirectoryService
    {
        public bool DetermineDirectory()
        {
            var browserDialog = new CommonOpenFileDialog();
            browserDialog.IsFolderPicker = true;
            browserDialog.Title = Title;
            browserDialog.InitialDirectory = InitialDirectory;

            if (browserDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryName = browserDialog.FileName;
                return true;
            }

            DirectoryName = string.Empty;
            return false;
        }

        public string FileName { get; set; }
        public string Filter { get; set; }
        public string DirectoryName { get; private set; }
        public bool ShowNewFolderButton { get; set; }
        public string InitialDirectory { get; set; }
        public string Title { get; set; }
    }
}