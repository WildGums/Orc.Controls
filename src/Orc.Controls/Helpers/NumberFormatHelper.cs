// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberFormatHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Linq;

    public static class NumberFormatHelper
    {
        #region Methods
        public static string GetFormat(int digits)
        {
            return new string(Enumerable.Repeat('0', digits).ToArray());
        }
        #endregion
    }
}
