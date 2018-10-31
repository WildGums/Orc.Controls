// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeAdjustmentProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public interface ITimeAdjustmentProvider
    {
        #region Methods
        TimeAdjustment GetTimeAdjustment(TimeAdjustmentStrategy strategy);
        #endregion
    }
}
