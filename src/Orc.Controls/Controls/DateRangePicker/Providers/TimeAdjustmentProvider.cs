namespace Orc.Controls;

using System;
using System.Collections.Generic;
using Catel;
using Catel.Services;

public class TimeAdjustmentProvider : ITimeAdjustmentProvider
{
    private readonly ILanguageService _languageService;

    private readonly Dictionary<TimeAdjustmentStrategy, TimeAdjustment> _timeAdjustments = new();

    public TimeAdjustmentProvider(ILanguageService languageService)
    {
        _languageService = languageService;
    }

    public TimeAdjustment GetTimeAdjustment(TimeAdjustmentStrategy strategy)
    {
        if (_timeAdjustments.TryGetValue(strategy, out var timeAdjustment))
        {
            return timeAdjustment;
        }

        timeAdjustment = CreateTimeAdjustment(strategy);
        _timeAdjustments[strategy] = timeAdjustment;

        return timeAdjustment;
    }

    private TimeAdjustment CreateTimeAdjustment(TimeAdjustmentStrategy strategy)
    {
        var timeAdjustment = new TimeAdjustment
        {
            Name = GetStrategyName(strategy),
            Strategy = strategy
        };

        return timeAdjustment;
    }

    private string GetStrategyName(TimeAdjustmentStrategy strategy)
    {
        return strategy switch
        {
            TimeAdjustmentStrategy.AdjustEndTime => _languageService.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_Adj_End_Time)),
            TimeAdjustmentStrategy.AdjustDuration => _languageService.GetRequiredString(nameof(Properties.Resources.Controls_DateRangePicker_Adj_Duration)),
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
        };
    }
}
