namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.FilterBox), ControlTypeName = nameof(ControlType.Edit))]
    public class FilterBox : Edit
    {
        public FilterBox(AutomationElement element) 
            : base(element)
        {
        }

        public FilterBoxMap Map => Map<FilterBoxMap>();
        public new FilterBoxModel Current => Model<FilterBoxModel>();

        public string Watermark
        {
            get => Map.WatermarkText?.Value ?? string.Empty;
        }

        public void Clear()
        {
            Map.ClearButton?.Click();
        }

        public void SelectItemFromSuggestionList(string item)
        {
            var list = GetSuggestionList();

            var listItem = list?.Items?.FirstOrDefault(x => Equals(x.DisplayText, item));

            listItem?.Select();
        }

        public List<string> OpenSuggestionList()
        {
            var list = GetSuggestionList();

            var result = list?.Items?.Select(x => x.DisplayText)
                .ToList() ?? new List<string>();

            return result;
        }

        private List GetSuggestionList()
        {
            SetFocus();

            KeyboardInputEx.Gesture(Key.LeftCtrl, Key.Space);

            Wait.UntilResponsive();

            var windowHost = Element.GetHostWindow();
            var popup = windowHost.Find(className: "Popup", controlType: ControlType.Window);
            var list = popup?.Find<List>();

            return list;
        }
    }
}
