// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeAdjustmentConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.IoC;
    using Catel.MVVM.Converters;

    public class TimeAdjustmentConverter : ValueConverterBase
    {
        #region Fields
        private readonly ITimeAdjustmentProvider _timeAdjustmentProvider;
        #endregion

        #region Constructors
        public TimeAdjustmentConverter()
        {
            _timeAdjustmentProvider = this.GetServiceLocator().ResolveType<ITimeAdjustmentProvider>();
        }
        #endregion

        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is TimeAdjustmentStrategy strategy)
            {
                return _timeAdjustmentProvider.GetTimeAdjustment(strategy);
            }

            return value;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            if (value is TimeAdjustment timeAdjustment)
            {
                return timeAdjustment.Strategy;
            }

            return base.ConvertBack(value, targetType, parameter);
        }
        #endregion
    }
}
