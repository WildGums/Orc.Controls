﻿namespace Orc.Controls;

using System.Windows;
using System;
using System.Windows.Markup;
using System.Windows.Media;

/// <summary>
/// This visual wrapper is used by VisualTargetPresentationSource
/// </summary>
[ContentProperty(nameof(Child))]
public class VisualWrapper : FrameworkElement
{
    private Visual? _child;

    /// <summary>
    /// Gets or sets the child.
    /// </summary>
    /// <value>The child.</value>
    public Visual? Child
    {
        get
        {
            return _child;
        }
        set
        {
            if (_child is not null)
            {
                RemoveVisualChild(_child);
            }

            _child = value;

            if (_child is not null)
            {
                AddVisualChild(_child);
            }
        }
    }

    /// <summary>
    /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.
    /// </summary>
    /// <param name="index">The zero-based index of the requested child element in the collection.</param>
    /// <returns>
    /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
    /// </returns>
    protected override Visual? GetVisualChild(int index)
    {
        if (_child is not null && index == 0)
        {
            return _child;
        }

        throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <summary>
    /// Gets the number of visual child elements within this element.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The number of visual child elements for this element.
    /// </returns>
    protected override int VisualChildrenCount
    {
        get { return _child is not null ? 1 : 0; }
    }
}
