namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;

    public interface ICallout
    {
        Guid Id { get; }

        string Name { get; }

        string Title { get; }

        object Tag { get; }

        bool IsOpen { get; }

        bool IsClosable { get; }

        TimeSpan ShowTime { get; set; }

        ICommand Command { get; }

        event EventHandler<CalloutEventArgs> Showing;
        event EventHandler<CalloutEventArgs> Hiding;

        void Show();
        void Hide();
    }
}
