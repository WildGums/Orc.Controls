namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using Orc.Automation.Controls;
    using AutomationEventArgs = Orc.Automation.AutomationEventArgs;

    public class SpinButton : FrameworkElement
    {
        private SpinButtonView _view;

        public SpinButton(AutomationElement element) 
            : base(element)
        {
        }

        public SpinButtonView View => _view ??= new SpinButtonView(Element);

        protected override void OnEvent(object sender, AutomationEventArgs args)
        {
            if (args.EventName == nameof(Controls.SpinButton.Increased))
            {
                Increased?.Invoke(this, EventArgs.Empty);

                return;
            }

            if (args.EventName == nameof(Controls.SpinButton.Decreased))
            {
                Decreased?.Invoke(this, EventArgs.Empty);

                return;
            }

            if (args.EventName == nameof(Controls.SpinButton.Canceled))
            {
                Canceled?.Invoke(this, EventArgs.Empty);

                return;
            }
        }

        public event EventHandler<EventArgs> Increased;
        public event EventHandler<EventArgs> Decreased;
        public event EventHandler<EventArgs> Canceled;
    }
}
