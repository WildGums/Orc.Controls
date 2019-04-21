// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringProperty.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.ComponentModel;
    using System.Data.Common;
    using Catel;
    using Catel.Data;

    public class ConnectionStringProperty : ObservableObject
    {
        #region Fields
        private readonly DbConnectionStringBuilder _dbConnectionStringBuilder;
        private readonly PropertyDescriptor _propertyDescriptor;
        #endregion

        #region Constructors
        public ConnectionStringProperty(bool isSensitive, DbConnectionStringBuilder dbConnectionStringBuilder, PropertyDescriptor propertyDescriptor)
        {
            Argument.IsNotNull(() => dbConnectionStringBuilder);
            Argument.IsNotNull(() => propertyDescriptor);

            _dbConnectionStringBuilder = dbConnectionStringBuilder;
            _propertyDescriptor = propertyDescriptor;
            Name = propertyDescriptor.DisplayName.ToUpperInvariant();
            IsSensitive = isSensitive;
        }
        #endregion

        #region Properties
        public string Name { get; }
        public bool IsSensitive { get; }

        public object Value
        {
            get => _propertyDescriptor.GetValue(_dbConnectionStringBuilder);

            set
            {
                var currentValue = _propertyDescriptor.GetValue(_dbConnectionStringBuilder);
                if (Equals(currentValue, value))
                {
                    return;
                }

                _propertyDescriptor.SetValue(_dbConnectionStringBuilder, value);
                RaisePropertyChanged(nameof(Value));
            }
        }
        #endregion
    }
}
