// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionState.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use ConnectionState from Orc.DataAccess library instead")]
    public enum ConnectionState
    {
        Undefined,
        Valid,
        Invalid
    }
}
