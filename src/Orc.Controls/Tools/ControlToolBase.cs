// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public abstract class ControlToolBase : IControlTool
    {
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
        public void Open()
        {
            if (IsOpened)
            {
                return;
            }

            OnOpen();

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
        protected abstract void OnOpen();

        private void RaiseClosedEvent()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
