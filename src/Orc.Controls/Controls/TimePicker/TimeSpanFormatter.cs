namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Documents;

    public class TimeSpanFormatter
    {
        public TimeSpanFormatter()
        {

        }

        public static string ChangeFormat(string dateTimeFormat)
        {
            string timeSpanFormat = String.Empty;
            for (int k = 0; k < dateTimeFormat.Length; k++)
            {
                char i = dateTimeFormat[k];
                if (i == 'h' || i == 'H' || i == 'm' || i == 'M' || i == 's' || i == 'S')
                {
                    timeSpanFormat = timeSpanFormat + i.ToString().ToLower();
                }
                else if (i != '\'' && i != 't' && i != 'T')
                {
                    timeSpanFormat = timeSpanFormat + @"'" + i + @"'";
                }
            }

            return timeSpanFormat;
        }
    }
}
