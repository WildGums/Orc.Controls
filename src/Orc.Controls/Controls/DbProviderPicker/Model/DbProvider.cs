// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public class DbProvider
    {
        #region Properties
        public string Name { get; set; }
        public string InvariantName { get; set; }
        public string Description { get; set; }
        #endregion

        #region Methods
        protected bool Equals(DbProvider other)
        {
            return string.Equals(InvariantName, other.InvariantName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DbProvider)obj);
        }

        public override int GetHashCode()
        {
            return (InvariantName != null ? InvariantName.GetHashCode() : 0);
        }
        #endregion
    }
}
