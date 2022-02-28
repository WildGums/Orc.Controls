namespace Orc.Controls.Automation
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using Orc.Automation;

    [AutomationAccessType]
    public class NumericTextBoxModel : AutomationControlModel
    {
        public NumericTextBoxModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }


        [ApiProperty]
        public string Text { get; set; }

        [ApiProperty]
        public string NullString { get; set; }

        [ApiProperty]
        public CultureInfo CultureInfo { get; set; }

        [ApiProperty]
        public bool IsChangeValueByUpDownKeyEnabled { get; set; }

        [ApiProperty]
        public bool IsNullValueAllowed { get; set; }

        [ApiProperty]
        public bool IsNegativeAllowed { get; set; }

        [ApiProperty]
        public bool IsDecimalAllowed { get; set; }

        [ApiProperty]
        public double MinValue { get; set; }

        [ApiProperty]
        public double MaxValue { get; set; }

        [ApiProperty]
        public string Format { get; set; }

        public double? Value
        {
            get => _accessor.GetValue<double?>(nameof(Value));
            set => _accessor.ExecuteAutomationMethod<SetNumericTextBoxValueRun>(value);
        }
    }

    //TODO: temporary solution
    public class SetNumericTextBoxValueRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            if (owner is not Controls.NumericTextBox numericTextBox)
            {
                return false;
            }

            var beenFocused = numericTextBox.IsKeyboardFocused;

            //We should move focus of the control
            //TODO:Vladimir:this is actually bug in control, but it prevent us from testing other controls based on this one, so... would be fixed lately
            if (beenFocused)
            {
                numericTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            var value = (double?)method.Parameters[0].ExtractValue();
            numericTextBox.SetCurrentValue(Controls.NumericTextBox.ValueProperty, value);

            if (beenFocused)
            {
                Keyboard.Focus(numericTextBox);
            }

            return true;
        }
    }
}
