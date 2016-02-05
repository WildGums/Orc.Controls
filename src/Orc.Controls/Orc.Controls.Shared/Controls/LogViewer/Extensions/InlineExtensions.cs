// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InlineExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Windows.Documents;
    using Catel;

    public static class InlineExtensions
    {
        #region Methods
        public static Bold Bold(this Inline inline)
        {
            Argument.IsNotNull(() => inline);

            return new Bold(inline);
        }

        public static Inline Append(this Inline inline, Inline inlineToAdd)
        {
            Argument.IsNotNull(() => inline);
            Argument.IsNotNull(() => inlineToAdd);

            var span = new Span(inline);

            span.Inlines.Add(inlineToAdd);

            return span;
        }

        public static Inline AppendRange(this Inline inline, IEnumerable<Inline> inlines)
        {
            Argument.IsNotNull(() => inline);
            Argument.IsNotNull(() => inlines);

            var span = new Span(inline);

            span.Inlines.AddRange(inlines);

            return span;
        }
        #endregion
    }
}