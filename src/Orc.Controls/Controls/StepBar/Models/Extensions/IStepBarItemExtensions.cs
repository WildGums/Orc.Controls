namespace Orc.Controls
{
    using Catel;

    public static class IStepBarItemExtensions
    {
        public static bool IsSkipped(this StepBarItemStates state)
        {
            if (Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsBeforeCurrent))
            {
                if (!Enum<StepBarItemStates>.Flags.IsFlagSet(state, StepBarItemStates.IsVisited))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
