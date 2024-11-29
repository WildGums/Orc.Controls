﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Automation;
using Catel.Logging;

/// <summary>
/// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
/// </summary>
public class StaggeredPanel : Panel
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private double _columnWidth;

    static StaggeredPanel()
    {
        HorizontalAlignmentProperty.OverrideMetadata(typeof(StaggeredPanel), new FrameworkPropertyMetadata((s, e) => OnHorizontalAlignmentChanged(s, e)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaggeredPanel"/> class.
    /// </summary>
    public StaggeredPanel()
    {

    }

    /// <summary>
    /// Gets or sets the desired width for each column.
    /// </summary>
    /// <remarks>
    /// The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch.
    /// </remarks>
    public double DesiredColumnWidth
    {
        get { return (double)GetValue(DesiredColumnWidthProperty); }
        set { SetValue(DesiredColumnWidthProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="DesiredColumnWidth"/> dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="DesiredColumnWidth"/> dependency property.</returns>
    public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
        nameof(DesiredColumnWidth),
        typeof(double),
        typeof(StaggeredPanel),
        new PropertyMetadata(250d, (sender, _) => ((StaggeredPanel)sender).InvalidateMeasure()));

    /// <summary>
    /// Gets or sets the distance between the border and its child object.
    /// </summary>
    /// <returns>
    /// The dimensions of the space between the border and its child as a Thickness value.
    /// Thickness is a structure that stores dimension values using pixel measures.
    /// </returns>
    public Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    /// <summary>
    /// Identifies the Padding dependency property.
    /// </summary>
    /// <returns>The identifier for the <see cref="Padding"/> dependency property.</returns>
    public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
        nameof(Padding),
        typeof(Thickness),
        typeof(StaggeredPanel),
        new PropertyMetadata(default(Thickness), (sender, _) => ((StaggeredPanel)sender).InvalidateMeasure()));

    /// <summary>
    /// Gets or sets the spacing between columns of items.
    /// </summary>
    public double ColumnSpacing
    {
        get { return (double)GetValue(ColumnSpacingProperty); }
        set { SetValue(ColumnSpacingProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="ColumnSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
        nameof(ColumnSpacing),
        typeof(double),
        typeof(StaggeredPanel),
        new PropertyMetadata(0d, (sender, _) => ((StaggeredPanel)sender).InvalidateMeasure()));

    /// <summary>
    /// Gets or sets the spacing between rows of items.
    /// </summary>
    public double RowSpacing
    {
        get { return (double)GetValue(RowSpacingProperty); }
        set { SetValue(RowSpacingProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="RowSpacing"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
        nameof(RowSpacing),
        typeof(double),
        typeof(StaggeredPanel),
        new PropertyMetadata(0d, (sender, _) => ((StaggeredPanel)sender).InvalidateMeasure()));

    /// <inheritdoc/>
    protected override Size MeasureOverride(Size availableSize)
    {
        // Shrink panel if no children
        if (Children.Count == 0)
        {
            return new Size(0, 0);
        }

        var availableWidth = availableSize.Width - Padding.Left - Padding.Right;
        var availableHeight = availableSize.Height - Padding.Top - Padding.Bottom;

        _columnWidth = Math.Min(DesiredColumnWidth, availableWidth);
        var numColumns = Math.Max(1, (int)Math.Floor(availableWidth / (_columnWidth + ColumnSpacing)));

        // adjust for column spacing on all columns expect the first
        var totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
        if (totalWidth > availableWidth)
        {
            numColumns--;
        }
        else if (double.IsInfinity(availableWidth))
        {
            availableWidth = totalWidth;
        }

        if (HorizontalAlignment == HorizontalAlignment.Stretch)
        {
            var occupiedSpacing = (numColumns - 1) * ColumnSpacing;
            if (availableWidth < occupiedSpacing)
            {
                Log.Debug($"Stretch Measure: availableWidth: {availableWidth}, spacing summary: {occupiedSpacing} [{numColumns - 1} x {ColumnSpacing}]");

                // Fallback value to avoid negative size ArgumentException
                occupiedSpacing = availableWidth;
            }

            availableWidth = availableWidth - occupiedSpacing;
            _columnWidth = availableWidth / numColumns;
        }

        var columnHeights = new double[numColumns];
        var itemsPerColumn = new double[numColumns];

        for (var i = 0; i < Children.Count; i++)
        {
            var columnIndex = GetColumnIndex(columnHeights);

            var child = Children[i];
            child.Measure(new Size(_columnWidth, availableHeight));
            var elementSize = child.DesiredSize;
            columnHeights[columnIndex] += elementSize.Height + (itemsPerColumn[columnIndex] > 0 ? RowSpacing : 0);
            itemsPerColumn[columnIndex]++;
        }

        double desiredHeight = columnHeights.Max();

        return new Size(availableWidth, desiredHeight);
    }

    /// <inheritdoc/>
    protected override Size ArrangeOverride(Size finalSize)
    {
        var horizontalOffset = Padding.Left;
        var verticalOffset = Padding.Top;

        if (_columnWidth == 0)
        {
            return finalSize;
        }

        var numColumns = Math.Max(1, (int)Math.Floor((finalSize.Width + ColumnSpacing) / (_columnWidth + ColumnSpacing)));

        // adjust for horizontal spacing on all columns expect the first
        var totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
        if (totalWidth > finalSize.Width)
        {
            numColumns--;

            // Need to recalculate the totalWidth for a correct horizontal offset
            totalWidth = _columnWidth + ((numColumns - 1) * (_columnWidth + ColumnSpacing));
        }

        switch (HorizontalAlignment)
        {
            case HorizontalAlignment.Right:
                horizontalOffset += finalSize.Width - totalWidth;
                break;

            case HorizontalAlignment.Center:
                horizontalOffset += (finalSize.Width - totalWidth) / 2;
                break;
        }

        var columnHeights = new double[numColumns];
        var itemsPerColumn = new double[numColumns];

        for (var i = 0; i < Children.Count; i++)
        {
            var columnIndex = GetColumnIndex(columnHeights);

            var child = Children[i];
            var elementSize = child.DesiredSize;

            var elementHeight = elementSize.Height;

            var itemHorizontalOffset = horizontalOffset + (_columnWidth * columnIndex) + (ColumnSpacing * columnIndex);
            var itemVerticalOffset = columnHeights[columnIndex] + verticalOffset + (RowSpacing * itemsPerColumn[columnIndex]);

            var bounds = new Rect(itemHorizontalOffset, itemVerticalOffset, _columnWidth, elementHeight);
            child.Arrange(bounds);

            columnHeights[columnIndex] += elementSize.Height;
            itemsPerColumn[columnIndex]++;
        }

        return base.ArrangeOverride(finalSize);
    }

    private static void OnHorizontalAlignmentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs dp)
    {
        if (sender is StaggeredPanel panel)
        {
            panel.InvalidateMeasure();
        }
    }

    private int GetColumnIndex(IReadOnlyList<double> columnHeights)
    {
        var columnIndex = 0;
        var height = columnHeights[0];

        for (var j = 1; j < columnHeights.Count; j++)
        {
            if (columnHeights[j] < height)
            {
                columnIndex = j;
                height = columnHeights[j];
            }
        }

        return columnIndex;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new StaggeredPanelAutomationPeer(this);
    }
}
