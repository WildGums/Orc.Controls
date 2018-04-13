// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WrapPanel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The wrap panel.
    /// </summary>
    public class WrapPanel : Panel
    {
        #region Methods
        /// <summary>
        /// The arrange override.
        /// </summary>
        /// <param name="finalSize">The final size.</param>
        /// <returns>The <see cref="Size" />.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double rowX, rowY;
            rowX = rowY = 0;

            double y = 0;

            foreach (FrameworkElement child in this.Children)
            {
                var ds = child.DesiredSize;
                y = rowY + ds.Height > y ? rowY + ds.Height : y;

                child.Arrange(new Rect(rowX, rowY, ds.Width, ds.Height));

                rowX += ds.Width;

                if (rowX + ds.Width > finalSize.Width)
                {
                    rowX = 0;
                    rowY = y;
                }
            }

            return base.ArrangeOverride( /*new Size(rowX, rowY + y)*/ finalSize);
        }

        /// <summary>
        /// The measure override.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The <see cref="Size" />.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (FrameworkElement child in this.Children)
            {
                child.Measure(availableSize);
            }

            double rowX, rowY;
            rowX = rowY = 0;

            double y = 0;
            double dsw = 0;
            double dsh = 0;

            foreach (FrameworkElement child in this.Children)
            {
                var ds = child.DesiredSize;
                dsw = ds.Width;
                dsh = ds.Height;
                y = rowY + ds.Height > y ? rowY + ds.Height : y;
                rowX += ds.Width;

                if (rowX + ds.Width > availableSize.Width)
                {
                    rowX = 0;
                    rowY = y;
                }
            }

            return new Size(rowX + dsw, rowY + dsh);
        }
        #endregion
    }
}