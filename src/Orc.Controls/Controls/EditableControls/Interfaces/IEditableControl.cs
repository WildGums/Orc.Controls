namespace Orc.Controls
{
    using System;

    public interface IEditableControl
    {
        #region Properties
        bool IsInEditMode { get; }
        #endregion

        event EventHandler<EventArgs> EditStarted;
        event EventHandler<EventArgs> EditEnded;
    }
}
