using System;
using System.Globalization;

namespace Kztek_Library.Helpers
{
    public class DatetimeHelper
    {
        public static DateTime ConvertString_DDMMYYYY_ToDate(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
