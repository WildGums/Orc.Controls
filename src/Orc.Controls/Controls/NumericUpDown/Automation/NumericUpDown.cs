namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;
    using Orc.Automation;

    public class NumericUpDown : FrameworkElement
    {
        private NumericUpDownView _view;

        public NumericUpDown(AutomationElement element)
            : base(element)
        {
        }

        public NumericUpDownView View => _view ??= new NumericUpDownView(Element);

        public Number Value
        {
            get => Access.GetValue<Number>();
            set => Access.SetValue(value);
        }

        public double MaxValue
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public double MinValue
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public int DecimalPlaces
        {
            get => Access.GetValue<int>();
            set => Access.SetValue(value);
        }

        public int MaxDecimalPlaces
        {
            get => Access.GetValue<int>();
            set => Access.SetValue(value);
        }

        public int MinDecimalPlaces
        {
            get => Access.GetValue<int>();
            set => Access.SetValue(value);
        }

        public bool IsDecimalPointDynamic
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public double MinorDelta
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public double MajorDelta
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public bool IsThousandSeparatorVisible
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public bool IsAutoSelectionActive
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }
    }
}
