// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InlineExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Windows.Documents;

    public static class InlineExtensions
    {
        #region Methods
        public static Bold Bold(this Inline inline)
        {
            return new Bold(inline);
        }

        public static Inline Append(this Inline inline, Inline inlineToAdd)
        {
            var span = new Span(inline);

            span.Inlines.Add(inlineToAdd);

            return span;
        }

        public static Inline AppendRange(this Inline inline, IEnumerable<Inline> inlines)
        {
            var span = new Span(inline);

            span.Inlines.AddRange(inlines);

            return span;
        }
        #endregion
    }
}