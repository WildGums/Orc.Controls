namespace Orc.Controls;

using Catel;

public static class IStepBarItemExtensions
{
    public static bool IsSkipped(this StepBarItemStates state)
    {
        if (!state.IsFlagSet(StepBarItemStates.IsBeforeCurrent))
        {
            return false;
        }

        return !state.IsFlagSet(StepBarItemStates.IsVisited);
    }

    public static bool IsFlagSet(this StepBarItemStates state, StepBarItemStates flag)
    {
        return Enum<StepBarItemStates>.Flags.IsFlagSet(state, flag);
    }
}
