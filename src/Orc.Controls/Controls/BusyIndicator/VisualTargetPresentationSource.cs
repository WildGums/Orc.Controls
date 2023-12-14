﻿namespace Orc.Controls;

using System.Windows;
using System.Windows.Media;

/// <summary>
/// Support for multi-threaded host visuals.
/// </summary>
/// <remarks>
/// The original code can be found here: http://blogs.msdn.com/b/dwayneneed/archive/2007/04/26/multithreaded-ui-hostvisual.aspx.
/// </remarks>
public class VisualTargetPresentationSource : PresentationSource
{
#pragma warning disable IDISP006 // Implement IDisposable.
    private readonly VisualTarget _visualTarget;
#pragma warning restore IDISP006 // Implement IDisposable.

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualTargetPresentationSource"/> class.
    /// </summary>
    /// <param name="hostVisual">The host visual.</param>
    public VisualTargetPresentationSource(HostVisual hostVisual)
    {
        _visualTarget = new VisualTarget(hostVisual);
    }

    /// <summary>
    /// When overridden in a derived class, gets or sets the root visual being presented in the source.
    /// </summary>
    /// <value></value>
    /// <returns>
    /// The root visual.
    /// </returns>
    public override Visual RootVisual
    {
        get { return _visualTarget.RootVisual; }
        set
        {
            var oldRoot = _visualTarget.RootVisual;

            // Set the root visual of the VisualTarget.  This visual will
            // now be used to visually compose the scene.
            _visualTarget.RootVisual = value;

            // Tell the PresentationSource that the root visual has
            // changed.  This kicks off a bunch of stuff like the
            // Loaded event.
            RootChanged(oldRoot, value);

            // Kickoff layout...
            if (value is not UIElement rootElement)
            {
                return;
            }

            rootElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            rootElement.Arrange(new Rect(rootElement.DesiredSize));
        }
    }

    /// <summary>
    /// When overridden in a derived class, gets a value that declares whether the object is disposed.
    /// </summary>
    /// <value></value>
    /// <returns>true if the object is disposed; otherwise, false.
    /// </returns>
    public override bool IsDisposed
    {
        get { return false; }
    }
  
    /// <summary>
    /// When overridden in a derived class, returns a visual target for the given source.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="T:System.Windows.Media.CompositionTarget"/> that is target for rendering the visual.
    /// </returns>
    protected override CompositionTarget GetCompositionTargetCore()
    {
        return _visualTarget;
    }
}
