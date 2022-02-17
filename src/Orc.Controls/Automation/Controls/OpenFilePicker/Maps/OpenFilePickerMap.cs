namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class OpenFilePickerMap : AutomationBase
    {
        public static string SelectedFileTextBoxId = nameof(SelectedFileTextBox);
        public static string OpenFileButtonId = nameof(OpenFileButton);
        public static string OpenDirectoryButtonId = nameof(OpenDirectoryButton);
        public static string ClearButtonId = nameof(ClearButton);

        public OpenFilePickerMap(AutomationElement element) 
            : base(element)
        {
        }

        public Edit SelectedFileTextBox => By.Id().One<Edit>();
        public Button OpenFileButton => By.Id().One<Button>();
        public Button OpenDirectoryButton => By.Id().One<Button>();
        public Button ClearButton => By.Id().One<Button>();
    }
}
