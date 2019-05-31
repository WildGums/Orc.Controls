// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataSourceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;

    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Deprecated")]
    public interface IDataSourceProvider
    {
        string DataBasesQuery { get; }
        IList<string> GetDataSources();
    }
}
