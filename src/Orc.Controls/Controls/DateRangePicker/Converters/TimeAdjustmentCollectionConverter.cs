namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel.MVVM.Converters;

public partial class TimeAdjustmentCollectionConverter : ValueConverterBase
{
    private readonly ITimeAdjustmentProvider _timeAdjustmentProvider;

    public TimeAdjustmentCollectionConverter(ITimeAdjustmentProvider timeAdjustmentProvider)
    {
        _timeAdjustmentProvider = timeAdjustmentProvider;
    }

    protected override object? Convert(object? value, Type targetType, object? parameter)
    {
        if (value is IEnumerable<TimeAdjustmentStrategy> timeAdjustmentStrategies)
        {
            return timeAdjustmentStrategies.Select(x => _timeAdjustmentProvider.GetTimeAdjustment(x));
        }

        return value;
    }
}
