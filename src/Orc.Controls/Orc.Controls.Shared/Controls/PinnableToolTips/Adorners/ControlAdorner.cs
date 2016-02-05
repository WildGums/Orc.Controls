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
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        #endregion

        #region Fields
        /// <summary>
        /// The child.
        /// </summary>
        private Control _child;

        /// <summary>
        /// The offset.
        /// </summary>
        private Point _offset;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        public Control Child
        {
            get { return _child; }

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

        /// <summary>
        /// Gets the child position.
        /// </summary>
        public Point ChildPosition { get; private set; }

        /// <summary>
        /// Gets or sets the horizontal offset.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Point Offset
        {
            get { return _offset; }

            set
            {
                _offset = value;
                InvalidateArrange();
            }
        }
        #endregion

        #region Methods
        protected override Size ArrangeOverride(Size finalSize)
        {
            var c = _child as IControlAdornerChild;
            Rect rect;

            if (Offset.X != 0 || Offset.Y != 0)
            {
                rect = new Rect(
                    ChildPosition.X + Offset.X,
                    ChildPosition.Y + Offset.Y,
                    finalSize.Width,
                    finalSize.Height);
            }
            else if (c != null)
            {
                ChildPosition = c.GetPosition();
                rect = new Rect(ChildPosition.X, ChildPosition.Y, finalSize.Width, finalSize.Height);
            }
            else
            {
                rect = new Rect(_offset.X, _offset.Y, finalSize.Width, finalSize.Height);
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