namespace Orc.Controls
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Catel;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase, ICallout
    {
        private readonly ICalloutManager _calloutManager;

        public CalloutViewModel(ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            _calloutManager = calloutManager;

            Id = Guid.NewGuid();

            ClosePopup = new Command(OnClosePopupExecute);
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public new string Title
        {
            get { return base.Title; }
            set { base.Title = value; }
        }

        public bool IsOpen { get; set; }

        public UIElement PlacementTarget { get; set; }

        public string Description { get; set; }

        public object InnerContent { get; set; }

        public int? Delay { get; set; }

        public object Tag => null;

        public bool IsClosable { get; set; }

        public TimeSpan ShowTime { get; set; }

        public ICommand Command { get; set; }

        public event EventHandler<CalloutEventArgs> Showing;

        public event EventHandler<CalloutEventArgs> Hiding;

        #region Commands
        public Command ClosePopup { get; private set; }

        private void OnClosePopupExecute()
        {
            if (IsClosable)
            {
                Hide();
            }
        }
        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _calloutManager.Register(this);
        }

        protected override async Task CloseAsync()
        {
            _calloutManager.Unregister(this);

            await base.CloseAsync();
        }

        public void Show()
        {
            if (IsOpen)
            {
                return;
            }

            IsOpen = true;
        }

        public void Hide()
        {
            if (!IsOpen)
            {
                return;
            }

            IsOpen = false;
        }

        private void OnIsOpenChanged()
        {
            if (IsOpen)
            {
                Showing?.Invoke(this, new CalloutEventArgs(this));
            }
            else
            {
                Hiding?.Invoke(this, new CalloutEventArgs(this));
            }
        }
    }
}
