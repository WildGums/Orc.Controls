// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringProperty.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Data.Common;
    using Catel;
    using Catel.Data;

    public class ConnectionStringProperty : ObservableObject
    {
        private readonly DbConnectionStringBuilder _dbConnectionStringBuilder;

        public ConnectionStringProperty(string name, DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            Argument.IsNotNull(() => dbConnectionStringBuilder);

            _dbConnectionStringBuilder = dbConnectionStringBuilder;
            Name = name;
        }

        public string Name { get; }

        public object Value
        {
            get { return _dbConnectionStringBuilder[Name]; }

            set
            {
                var name = Name;
                if (_dbConnectionStringBuilder[name] == value)
                {
                    return;
                }

                _dbConnectionStringBuilder[name] = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}