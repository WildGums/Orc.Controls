namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using AutomationEventArgs = System.Windows.Automation.AutomationEventArgs;

    [AutomatedControl(Class = typeof(Controls.OpenFilePicker))]
    public class OpenFilePicker : FrameworkElement<OpenFilePickerModel, OpenFilePickerMap>
    {
        private AutomationElement _openDialog;

        public OpenFilePicker(AutomationElement element)
            : base(element)
        {
        }

        public string SelectedFileDisplayPath
        {
            get => Map.SelectedFileTextBox.Text;
        }

        public List<string> Filters
        {
            get
            {
                var dialog = OpenFileDialog();
                dialog.Element.SetFocus();
                var filters = dialog.Filters;

                dialog.Cancel();

                return filters;
            }
        }

        public void Clear()
        {
            Map.ClearButton.Click();
        }

        public void SelectFile(string filePath)
        {
            var dialog = OpenFileDialog();

            dialog.Element.SetFocus();

            dialog.FilePath = filePath;

            dialog.Accept();
        }

        public void OpenContainingDirectory()
        {
            var map = Map;

            map.OpenDirectoryButton?.Click();
        }

        public OpenFileDialog OpenFileDialog()
        {
            var map = Map;
            
            var hostWindow = Element.GetHostWindow();

            hostWindow.DialogOpened += OnDialogOpened;

            map.OpenFileButton.Click();

            Wait.UntilResponsive(1000);

            hostWindow.DialogOpened -= OnDialogOpened;

            return _openDialog.As<OpenFileDialog>();
        }

        private void OnDialogOpened(object sender, AutomationEventArgs e)
        {
            if (sender is not AutomationElement dialog)
            {
                return;
            }

            _openDialog = dialog;
        }
    }
}
