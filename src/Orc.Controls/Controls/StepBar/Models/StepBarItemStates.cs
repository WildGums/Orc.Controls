
namespace Orc.Controls
{
    using System;

    [Flags]
    public enum StepBarItemStates : short
    {
        None = 0,
        IsVisited = 1,
        IsOptional = 2,
        IsLast = 4,
    }
}
