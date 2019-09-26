// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public abstract class ControlToolBase : IControlTool
    {
        #region Fields
        protected object Target;
        #endregion

        #region Properties
        public abstract string Name { get; }
        public bool IsOpened { get; private set; }
        #endregion

        #region IControlTool Members
        public virtual void Attach(object target)
        {
            Target = target;

            Attached?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Detach()
        {
            Target = null;

            Detached?.Invoke(this, EventArgs.Empty);
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use Open() with parameter instead")]
        public void Open()
        {
            Open(null);
        }

        public void Open(object parameter = null)
        {
            if (IsOpened)
            {
                return;
            }

            Opening?.Invoke(this, EventArgs.Empty);

            OnOpen(parameter);

            IsOpened = true;
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Close()
        {
            IsOpened = false;

            Closed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Attached;
        public event EventHandler<EventArgs> Detached;
        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Opened;
        public event EventHandler<EventArgs> Opening;
        #endregion

        #region Methods
        protected abstract void OnOpen(object parameter = null);
        #endregion
    }
}
