namespace Orc.Controls
{
    using System;

    [Flags]
    public enum StepBarItemStates : short
    {
        None = 0,
        IsVisited = 1,
        IsOptional = 2,
        IsCurrent = 4,
        IsBeforeCurrent = 8,
        IsLast = 16,
    }
}
