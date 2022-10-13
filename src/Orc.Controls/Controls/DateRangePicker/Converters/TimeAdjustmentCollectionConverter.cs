// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeAdjustmentCollectionConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel.IoC;
    using Catel.MVVM.Converters;

    public class TimeAdjustmentCollectionConverter : ValueConverterBase
    {
        #region Fields
        private readonly ITimeAdjustmentProvider _timeAdjustmentProvider;
        #endregion

        #region Constructors
        public TimeAdjustmentCollectionConverter()
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            _timeAdjustmentProvider = this.GetServiceLocator().ResolveType<ITimeAdjustmentProvider>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        }
        #endregion

        #region Methods
        protected override object? Convert(object? value, Type targetType, object? parameter)
        {
            if (value is IEnumerable<TimeAdjustmentStrategy> timeAdjustmentStrategies)
            {
                return timeAdjustmentStrategies.Select(x => _timeAdjustmentProvider.GetTimeAdjustment(x));
            }

            return value;
        }
        #endregion
    }
}
