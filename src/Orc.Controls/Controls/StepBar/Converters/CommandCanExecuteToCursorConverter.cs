namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.MVVM.Converters;

    public class CommandCanExecuteToCursorConverter : ValueConverterBase
    {
        public CommandCanExecuteToCursorConverter()
        {
        }

        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value is Catel.MVVM.TaskCommand<IStepBarItem> command)
            {
                return command.CanExecute() ? System.Windows.Input.Cursors.Hand : null;
            }

            return null;
        }
    }
}
