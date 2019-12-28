// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStateToColorBrushValueConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.MVVM.Converters;

    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use ConnectionStateToColorBrushValueConverter from Orc.DataAccess.Xaml library instead")]
    public class ConnectionStateToColorBrushValueConverter : ValueConverterBase<ConnectionState, SolidColorBrush>
    {
        protected override object Convert(ConnectionState value, Type targetType, object parameter)
        {
            switch (value)
            {
                case ConnectionState.Undefined:
                    return new SolidColorBrush(Colors.Gray);
                case ConnectionState.Valid:
                    return new SolidColorBrush(Colors.Green);
                case ConnectionState.Invalid:
                    return new SolidColorBrush(Colors.Red);
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
