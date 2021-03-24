namespace Orc.Controls
{
    using System;
    using Catel.MVVM.Converters;

    public class CommandCanExecuteToBooleanConverter : ValueConverterBase
    {
        public CommandCanExecuteToBooleanConverter()
        {
        }

        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is Catel.MVVM.TaskCommand<IStepBarItem> command)
            {
                return command.CanExecute();
            }

            return false;
        }
    }
}
