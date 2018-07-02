// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationFilterGroupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls.Services
{
    using System.Collections.Generic;
    using Orc.Controls.Models;

    public interface IApplicationFilterGroupService
    {
        IEnumerable<LogFilterGroup> Load();
        void Save(IEnumerable<LogFilterGroup> filterGroups);
    }
}
