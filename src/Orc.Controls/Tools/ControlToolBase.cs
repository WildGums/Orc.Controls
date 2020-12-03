// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlToolBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Data;

    public abstract class ControlToolBase : ModelBase, IControlTool
    {
        #region Fields
        protected object _target;
        #endregion

        #region Properties
        public abstract string Name { get; }
        public bool IsOpened { get; private set; }
        public virtual bool IsEnabled => true;
        #endregion

        #region IControlTool Members
        public virtual void Attach(object target)
        {
            _target = target;

            Attached?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Detach()
        {
            _target = null;

            Detached?.Invoke(this, EventArgs.Empty);
        }

        public void Open(object parameter = null)
        {
            if (IsOpened)
            {
                OnAddParameter(parameter);

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
        protected virtual void OnAddParameter(object parameter)
        {
        }

        protected abstract void OnOpen(object parameter = null);
        #endregion
    }
}
