// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionStringBuilderServiceInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public interface IConnectionStringBuilderServiceInitializer
    {
        void Initialize(IConnectionStringBuilderService connectionStringBuilderService);
    }
}