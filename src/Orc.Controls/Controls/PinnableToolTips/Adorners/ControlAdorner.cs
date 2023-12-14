namespace Orc.Controls;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Catel;
using Catel.Reflection;

/// <summary>
/// An adorner class that contains a control as only child.
/// </summary>
internal class ControlAdorner : Adorner
{
    private Control? _child;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlAdorner" /> class.
    /// </summary>
    /// <param name="adornedElement">The adorned element.</param>
    public ControlAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        ChildPosition = new Point();
    }

    /// <summary>
    /// Gets the visual children count.
    /// </summary>
    protected override int VisualChildrenCount => 1;

    /// <summary>
    /// Gets or sets the child.
    /// </summary>
    public Control? Child
    {
        get => _child;

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

    public Point ChildPosition { get; private set; }

    public Point Offset { get; private set; }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_child is null)
        {
            return Size.Empty; 
        }

        PropertyHelper.TryGetPropertyValue(_child, nameof(PinnableToolTip.HorizontalOffset), out double childHorizontalOffset);
        PropertyHelper.TryGetPropertyValue(_child, nameof(PinnableToolTip.VerticalOffset), out double childVerticalOffset);

        Offset = new Point(childHorizontalOffset, childVerticalOffset);

        Rect rect;

        if (_child is IControlAdornerChild adornerChild)
        {
            var childPosition = adornerChild.GetPosition();

            //Debug.WriteLine($"Old child position: {ChildPosition}");
            //Debug.WriteLine($"New child position: {childPosition}");

            ChildPosition = childPosition;

            var finalPosition = new Point(childPosition.X + childHorizontalOffset,
                childPosition.Y + childVerticalOffset);

            rect = new Rect(finalPosition.X, finalPosition.Y, finalSize.Width, finalSize.Height);
        }
        else
        {
            rect = new Rect(Offset.X, Offset.Y, finalSize.Width, finalSize.Height);
        }

        _child.Arrange(rect);

        return new Size(_child.ActualWidth, _child.ActualHeight);
    }

    protected override Visual? GetVisualChild(int index)
    {
        Argument.IsNotOutOfRange("index", index, 0, 0);

        return _child;
    }

    protected override Size MeasureOverride(Size constraint)
    {
        if (_child is null)
        {
            return Size.Empty;
        }

        _child.Measure(constraint);
        return _child.DesiredSize;
    }
}
