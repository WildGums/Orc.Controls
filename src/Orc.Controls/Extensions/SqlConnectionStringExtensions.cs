// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionStringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public static class SqlConnectionStringExtensions
    {
        #region Methods
        public static ConnectionStringProperty TryGetProperty(this SqlConnectionString connectionString, string propertyName)
        {
            var properties = connectionString?.Properties;
            if (properties == null)
            {
                return null;
            }

            var upperInvariantPropertyName = propertyName.ToUpperInvariant();
            return !properties.ContainsKey(upperInvariantPropertyName) ? null : properties[upperInvariantPropertyName];
        }
        #endregion
    }
}
