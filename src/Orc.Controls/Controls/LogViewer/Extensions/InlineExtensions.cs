namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Documents;

    public static class InlineExtensions
    {
        public static Bold Bold(this Inline inline)
        {
            ArgumentNullException.ThrowIfNull(inline);

            return new Bold(inline);
        }

        public static Inline Append(this Inline inline, Inline inlineToAdd)
        {
            ArgumentNullException.ThrowIfNull(inline);
            ArgumentNullException.ThrowIfNull(inlineToAdd);

            var span = new Span(inline);

            span.Inlines.Add(inlineToAdd);

            return span;
        }

        public static Inline AppendRange(this Inline inline, IEnumerable<Inline> inlines)
        {
            ArgumentNullException.ThrowIfNull(inline);
            ArgumentNullException.ThrowIfNull(inlines);

            var span = new Span(inline);

            span.Inlines.AddRange(inlines);

            return span;
        }
    }
}
