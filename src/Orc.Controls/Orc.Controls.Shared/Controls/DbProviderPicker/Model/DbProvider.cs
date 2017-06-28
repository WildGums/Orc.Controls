// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public class DbProvider
    {
        public string Name { get; set; }
        public string InvariantName { get; set; }
        public string Description { get; set; }

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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((DbProvider) obj);
        }

        public override int GetHashCode()
        {
            return (InvariantName != null ? InvariantName.GetHashCode() : 0);
        }
    }
}