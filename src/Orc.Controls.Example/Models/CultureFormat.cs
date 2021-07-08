// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CultureFormat.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orc.Controls.Example.Models
{
    using System.Globalization;

    public class CultureFormat
    {
        public CultureInfo Culture { get; set; }
        public string CultureCode => $"[{Culture?.IetfLanguageTag}]";
        public string FormatValue { get; set; }
    }
}
