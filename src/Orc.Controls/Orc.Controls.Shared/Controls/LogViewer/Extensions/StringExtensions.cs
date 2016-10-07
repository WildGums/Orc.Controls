// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Documents;

    public static class StringExtensions
    {
        #region Methods
        public static Inline ToInline(this string text)
        {
            return new Run(text);
        }
        #endregion
    }
}