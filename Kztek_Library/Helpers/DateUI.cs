using System;

namespace Kztek_Library.Helpers
{
    // Dinh dang thoi gian
    public enum DatetimeFormat
    {
        DDMMYY = 1, // Ngay, thang, nam
        YYMMDD = 2, // Nam, thang, ngay
        MMDDYY = 3  // Thang, ngay, nam
    }

    public enum DateInterval
    {
        Year,
        Month,
        Weekday,
        Day,
        Hour,
        Minute,
        Second
    } 

    public class DateUI
    {
        // Chuyen doi dinh dang thoi gian
        public static string ToShortDateString(DateTime dt, DatetimeFormat format)
        {
            switch (format)
            {
                case DatetimeFormat.DDMMYY:
                    return dt.Day + "/" + dt.Month + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute;
                case DatetimeFormat.MMDDYY:
                    return dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute;
                case DatetimeFormat.YYMMDD:
                    return dt.Year + "/" + dt.Month + "/" + dt.Day + " " + dt.Hour + ":" + dt.Minute;
            }
            return "";
        }

        // Chuyen doi dinh dang thoi gian
        public static string ToShortDateString(string date, DatetimeFormat format)
        {
            DateTime dt = Convert.ToDateTime(date);
            switch (format)
            {
                case DatetimeFormat.DDMMYY:
                    return dt.Day + "/" + dt.Month + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute;
                case DatetimeFormat.MMDDYY:
                    return dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute;
                case DatetimeFormat.YYMMDD:
                    {
                        return dt.Year + "/" + dt.Month + "/" + dt.Day + " " + dt.Hour + ":" + dt.Minute;
                    }
            }
            return "";
        }

        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            int datediff = 0;
            for (int i = 1; i < 5000; i++)
            {
                if (date1.AddDays(i).ToString("yyyy/MM/dd") == date2.ToString("yyyy/MM/dd"))
                {
                    datediff = i;
                    break;
                }
            }
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return datediff / 7;
                case DateInterval.Day:
                    return datediff; // Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }

        private static long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        public static string GetDayOfWeek(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Thứ 2";
                case DayOfWeek.Tuesday:
                    return "Thứ 3";
                case DayOfWeek.Wednesday:
                    return "Thứ 4";
                case DayOfWeek.Thursday:
                    return "Thứ 5";
                case DayOfWeek.Friday:
                    return "Thứ 6";
                case DayOfWeek.Saturday:
                    return "Thứ 7";
                case DayOfWeek.Sunday:
                    return "Chủ nhật";
            }
            return "";
        }

        // Get day of week
        public static int GetDayOfWeekInNumber(DateTime dates)
        {
            switch (dates.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 1;
                case DayOfWeek.Monday:
                    return 2;
                case DayOfWeek.Tuesday:
                    return 3;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 5;
                case DayOfWeek.Friday:
                    return 6;
                case DayOfWeek.Saturday:
                    return 7;
                default:
                    return -1;
            }
        }

        // Get day of week
        public static int GetDayOfWeekInNumber1(DateTime dates)
        {
            switch (dates.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default:
                    return -1;
            }
        }

        // Get day of week
        public static int GetDayOfWeekInNumber2(DateTime dates)
        {
            switch (dates.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                default:
                    return -1;
            }
        }

        // Get day of week (DS3000)
        public static int GetDayOfWeekInNumberDS3000(DateTime dates)
        {
            switch (dates.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 32;
                case DayOfWeek.Tuesday:
                    return 64;
                case DayOfWeek.Wednesday:
                    return 96;
                case DayOfWeek.Thursday:
                    return 128;
                case DayOfWeek.Friday:
                    return 160;
                case DayOfWeek.Saturday:
                    return 192;
                default:
                    return 0;
            }
        }

        // Get time number in Minute (time string format HH:MM)
        public static int GetTimeNumber(string timeString)
        {
            return Convert.ToInt32(timeString.Substring(0, 2)) * 60 + Convert.ToInt32(timeString.Substring(3, 2));
        }

        // Get time string(time string format HH:MM)
        public static string GetTimeString(int timeNumber)
        {
            int hour = timeNumber / 60;
            int minute = timeNumber - hour * 60;
            return (hour.ToString("00") + ":" + minute.ToString("00"));
        }

        public static long DateDiff_108(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            int datediff = 0;
            for (int i = 1; i < 5000; i++)
            {
                if (date1.AddDays(i) >= DateTime.Parse(date2.ToString("yyyy/MM/dd 03:30")))
                {
                    datediff = i;
                    break;
                }
            }
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return datediff / 7;
                case DateInterval.Day:
                    return datediff; // Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }
    }
}
