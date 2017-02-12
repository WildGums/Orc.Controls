// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberFormatHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class NumerFormatHelper
    {
        public static string GetFormat(int digits)
        {
            return new string(Enumerable.Repeat('0', digits).ToArray());
        }
    }
}
