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

        #region Constructors
        protected ControlToolBase()
        {
        }
        #endregion

        #region Properties
        public abstract string Name { get; }
        public bool IsOpened { get; private set; }
        #endregion

        #region IControlTool Members
        public virtual void Attach(object target)
        {
            Target = target;
        }

        public virtual void Detach()
        {
            Target = null;
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

            OnOpen(parameter);

            IsOpened = true;
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Close()
        {
            IsOpened = false;

            RaiseClosedEvent();
        }

        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Opened;
        #endregion

        #region Methods
        protected abstract void OnOpen(object parameter = null);

        private void RaiseClosedEvent()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
