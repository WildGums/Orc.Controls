namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class FindReplaceViewMap : AutomationBase
    {
        private Tab _tab;

        public FindReplaceViewMap(AutomationElement element) 
            : base(element)
        {
        }

        public override By By => new(Element, Tab);
        public Tab Tab => _tab ??= base.By.Id("tabMain").One<Tab>();

        public Button FindNextButton  => By.Tab(0).Id().One<Button>();
        public Edit FindEdit => By.Tab(0).Id("TxtFind").One<Edit>();

        public Edit FindReplaceEdit => By.Tab(1).Id("TxtFind2").One<Edit>();
        public Edit ReplaceEdit => By.Tab(1).Id("TxtReplace").One<Edit>();
        public Button FindReplaceNextButton => By.Tab(1).Id().One<Button>();
        public Button ReplaceButton => By.Tab(1).Id().One<Button>();
        public Button ReplaceAllButton => By.Tab(1).Id().One<Button>();

        public CheckBox CaseSensitiveCheckBox => By.Id("CbCaseSensitive").One<CheckBox>();
        public CheckBox WholeWordCheckBox => By.Id("CbWholeWord").One<CheckBox>(); 
        public CheckBox RegexCheckBox => By.Id("CbRegex").One<CheckBox>();
        public CheckBox WildcardsCheckBox => By.Id("CbWildcards").One<CheckBox>();
        public CheckBox SearchUpCheckBox => By.Id("CbSearchUp").One<CheckBox>();
    }
}
