namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class Expander : FrameworkElement
    {
        private ExpanderView _view;

        public Expander(AutomationElement element) 
            : base(element)
        {
        }

        public ExpanderView View => _view ??= new ExpanderView(Element);

        public bool IsExpanded
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public ExpandDirection ExpandDirection
        {
            get => Access.GetValue<ExpandDirection>();
            set => Access.SetValue(value);
        }

        public double ExpandDuration
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public bool AutoResizeGrid
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }
    }
}
