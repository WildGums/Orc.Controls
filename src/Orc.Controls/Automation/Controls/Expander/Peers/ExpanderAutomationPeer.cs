#nullable disable
namespace Orc.Controls.Automation;

using System.Linq;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using Catel.Windows;
using Orc.Automation;

public class ExpanderAutomationPeer : AutomationControlPeerBase<Controls.Expander>, IExpandCollapseProvider
{
    public ExpanderAutomationPeer(Controls.Expander owner)
        : base(owner)
    {
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        return patternInterface is PatternInterface.ExpandCollapse 
            ? this 
            : base.GetPattern(patternInterface);
    }

    #region IExpandCollapseProvider methods
    public void Expand()
    {
        var toggleProvider = GetToggleProvider();
        if (toggleProvider?.ToggleState == ToggleState.Off)
        {
            toggleProvider.Toggle();
        }
    }

    public void Collapse()
    {
        var toggleProvider = GetToggleProvider();
        if (toggleProvider?.ToggleState == ToggleState.On)
        {
            toggleProvider.Toggle();
        }
    }

    public ExpandCollapseState ExpandCollapseState
        => GetToggleProvider()?.ToggleState == ToggleState.On
            ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;
    #endregion

    private IToggleProvider GetToggleProvider()
    {
        //TODO:Vladimir: Don't know why FindVisualDescendant doesn't work
        //This is a temporary solution
        var toggleButton = Control.GetChildren().ElementAt(0)
            .GetChildren().ElementAt(0)
            .GetChildren().ElementAt(0)
            .GetChildren().ElementAt(0) as ToggleButton;

        //var toggleButton = Control.FindVisualDescendantWithAutomationId("HeaderSiteToggleButton") as ToggleButton;
        if (toggleButton is null)
        {
            return null;
        }

        var peer = new ToggleButtonAutomationPeer(toggleButton);
        return peer.GetPattern(PatternInterface.Toggle) as IToggleProvider;
    }
}
#nullable enable
