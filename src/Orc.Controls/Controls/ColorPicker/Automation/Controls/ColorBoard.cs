namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using AutomationEventArgs = Orc.Automation.AutomationEventArgs;

    public class ColorBoard : FrameworkElement
    {
        public ColorBoard(AutomationElement element)
            : base(element)
        {
            View = new ColorBoardView(this);
        }

        public Color Color
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        protected override void OnEvent(object sender, AutomationEventArgs args)
        {
            if (args.EventName == nameof(Controls.ColorBoard.DoneClicked))
            {
                DoneClicked?.Invoke(this, EventArgs.Empty);

                return;
            }

            if (args.EventName == nameof(Controls.ColorBoard.CancelClicked))
            {
                CancelClicked?.Invoke(this, EventArgs.Empty);

                return;
            }
        }

        public event EventHandler<EventArgs> DoneClicked;
        public event EventHandler<EventArgs> CancelClicked;

        public ColorBoardView View { get; }
    }
}
