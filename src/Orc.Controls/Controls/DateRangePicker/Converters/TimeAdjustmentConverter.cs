namespace Orc.Controls;

using System;
using Catel.IoC;
using Catel.MVVM.Converters;

public partial class TimeAdjustmentConverter : ValueConverterBase
{
    private readonly ITimeAdjustmentProvider _timeAdjustmentProvider;

    public TimeAdjustmentConverter(ITimeAdjustmentProvider timeAdjustmentProvider)
    {
        _timeAdjustmentProvider = timeAdjustmentProvider;
    }

    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (value is TimeAdjustmentStrategy strategy)
        {
            return _timeAdjustmentProvider.GetTimeAdjustment(strategy);
        }

        return value;
    }

    protected override object? ConvertBack(object? value, Type targetType, object? parameter)
    {
        if (value is TimeAdjustment timeAdjustment)
        {
            return timeAdjustment.Strategy;
        }

        return base.ConvertBack(value, targetType, parameter);
    }
}
