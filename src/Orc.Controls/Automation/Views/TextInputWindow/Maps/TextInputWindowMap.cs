namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class TextInputWindowMap : AutomationBase
    {
        public TextInputWindowMap(AutomationElement element) 
            : base(element)
        {
        }

        public Edit TextEdit => By.One<Edit>();

        public Button OkButton=> By.Name("OK").One<Button>();
        public Button CancelButton => By.Name("Cancel").One<Button>();
    }
}
