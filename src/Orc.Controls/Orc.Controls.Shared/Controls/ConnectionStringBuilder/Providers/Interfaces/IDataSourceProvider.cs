// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSourceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;

    public interface IDataSourceProvider
    {
        string GetAllDataSourceBasesQuery { get; }
        IList<string> GetDataSources();
    }
}