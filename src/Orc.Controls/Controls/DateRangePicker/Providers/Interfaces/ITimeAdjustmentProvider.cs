namespace Orc.Controls;

public interface ITimeAdjustmentProvider
{
    TimeAdjustment GetTimeAdjustment(TimeAdjustmentStrategy strategy);
}
