// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlAdorner.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
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
        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAdorner" /> class.
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
        public ControlAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            ChildPosition = new Point();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the visual children count.
        /// </summary>
        protected override int VisualChildrenCount => 1;
        #endregion

        #region Fields
        /// <summary>
        /// The child.
        /// </summary>
        private Control _child;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        public Control Child
        {
            get => _child;

            set
            {
                if (_child != null)
                {
                    RemoveVisualChild(_child);
                }

                _child = value;

                if (_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        public Point ChildPosition { get; private set; }

        public Point Offset { get; private set; }
        #endregion

        #region Methods
        protected override Size ArrangeOverride(Size finalSize)
        {
            var c = _child as IControlAdornerChild;

            PropertyHelper.TryGetPropertyValue(c, "HorizontalOffset", out double childHorizontalOffset);
            PropertyHelper.TryGetPropertyValue(c, "VerticalOffset", out double childVerticalOffset);
             
            Offset = new Point(childHorizontalOffset, childVerticalOffset);

            Rect rect;
            if (c != null)
            {
                var childPosition = c.GetPosition();
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

        protected override Visual GetVisualChild(int index)
        {
            Argument.IsNotOutOfRange("index", index, 0, 0);

            return _child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);

            return _child.DesiredSize;
        }
        #endregion
    }
}
