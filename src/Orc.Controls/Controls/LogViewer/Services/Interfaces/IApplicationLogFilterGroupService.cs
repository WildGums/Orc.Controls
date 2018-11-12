// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationLogFilterGroupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApplicationLogFilterGroupService
    {
        #region Methods
        Task<IEnumerable<LogFilterGroup>> LoadAsync();

        Task SaveAsync(IEnumerable<LogFilterGroup> filterGroups);
        #endregion
    }
}
