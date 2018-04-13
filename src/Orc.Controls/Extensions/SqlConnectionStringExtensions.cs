// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionStringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public static class SqlConnectionStringExtensions
    {
        public static ConnectionStringProperty TryGetProperty(this SqlConnectionString connectionString, string propertyName)
        {
            var properties = connectionString?.Properties;
            if (properties == null)
            {
                return null;
            }

            return !properties.ContainsKey(propertyName) ? null : properties[propertyName];
        }
    }
}