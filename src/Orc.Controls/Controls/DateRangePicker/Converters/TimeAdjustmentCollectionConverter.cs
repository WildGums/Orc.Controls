namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel.IoC;
using Catel.MVVM.Converters;

public class TimeAdjustmentCollectionConverter : ValueConverterBase
{
    private readonly ITimeAdjustmentProvider _timeAdjustmentProvider;

    public TimeAdjustmentCollectionConverter()
    {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
        _timeAdjustmentProvider = this.GetServiceLocator().ResolveRequiredType<ITimeAdjustmentProvider>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
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
