namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Menu))]
    public class ColorLegendSettingsControl : Menu
    {
        public ColorLegendSettingsControl(AutomationElement element) 
            : base(element)
        {
        }

        private CheckBox ShowVisibilityColumnCheckBox => By.Id("ShowVisibilityColumnMenuItem").One<MenuItem>().By.One<CheckBox>();
        private CheckBox AllowColorEditCheckBox => By.Id("AllowColorEditMenuItem").One<MenuItem>().By.One<CheckBox>();
        private CheckBox ShowColorsCheckBox => By.Id("ShowColorsMenuItem").One<MenuItem>().By.One<CheckBox>();
        
        public bool IsVisibilityColumnVisible
        {
            get => (bool) ShowVisibilityColumnCheckBox.IsChecked;
            set => ShowVisibilityColumnCheckBox.IsChecked = value;
        }

        public bool IsColorEditAllowed
        {
            get => (bool)AllowColorEditCheckBox.IsChecked;
            set => AllowColorEditCheckBox.IsChecked = value;
        }

        public bool IsColorsVisible
        {
            get => (bool)ShowColorsCheckBox.IsChecked;
            set => ShowColorsCheckBox.IsChecked = value;
        }
    }
}