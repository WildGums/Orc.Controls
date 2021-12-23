namespace Orc.Automation.Controls
{
    using System.Windows.Automation;
    using System.Windows.Media;
    using Catel.Windows.Interactivity;

    public class FrameworkElement : AutomationControl
    {
        public FrameworkElement(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
        }

        #region Automation Properties
        public AutomationElement.AutomationElementInformation AutomationProperties => Element.Current;
        #endregion

        public void AttachBehavior<TBehavior>()
            where TBehavior : IBehavior
        {
            Access.AttachBehavior(typeof(TBehavior));
        }

        public bool IsEnabled => AutomationProperties.IsEnabled;

        public SolidColorBrush Background
        {
            get => Access.GetValue<SolidColorBrush>();
            set => Access.SetValue(value);
        }

        public SolidColorBrush BorderBrush
        {
            get => Access.GetValue<SolidColorBrush>();
            set => Access.SetValue(value);
        }

        public SolidColorBrush Foreground
        {
            get => Access.GetValue<SolidColorBrush>();
            set => Access.SetValue(value);
        }

        public double ActualWidth
        {
            get => Access.GetValue<double>();
        }

        public double ActualHeight
        {
            get => Access.GetValue<double>();
        }

        public bool Focusable
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        public void SetFocus()
        {
            Element?.SetFocus();
        }

        public object TryFindResource(string resourceKey)
        {
            return Access.Execute(nameof(TryFindResource), resourceKey);
        }
    }
}
