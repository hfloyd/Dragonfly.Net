namespace Dragonfly.NetHelpers
{
    using System;

    public static class Dates
    {
        private const string ThisClassName = "Dragonfly.NetHelpers.Dates";

        enum ConcatDateFormat
        {
            MMDDYYYY,
            YYYYMMDD
        }

        public static string FormatMonthNumber(int MonthNumber, string Format)
        {
            var tempDate = Convert.ToDateTime(String.Concat(MonthNumber, "/1/", DateTime.Now.Year.ToString()));

            return tempDate.ToString(Format);
        }

        public static string ConcatenateDate(string Month, string Day, string Year, string DateFormat = "MM/dd/yyyy")
        {
            string ReturnString = "";
            string DateString = String.Concat(Month.PadLeft(2, '0'), "/", Day.PadLeft(2, '0'), "/", Year);
            DateTime DateTest;
            try
            {
                //BirthdateTest = Convert.ToDateTime(BirthdateString);
                DateTest = DateTime.ParseExact(DateString, "MM/dd/yyyy", null);

                string DateTestString = DateTest.ToString("MM/dd/yyyy");

                if (DateTestString == DateString)
                {
                    ReturnString = DateString;
                }
                else
                {
                    var functionName = string.Format("{0}.ConcatenateDate", ThisClassName);
                    var msg = string.Format("{0} : DateString={1} NOT EQUAL TO DateTest={2} (DateTestString={3})", functionName, DateString, DateTest, DateTestString);
                    //Info.LogInfo(msg);
                    ReturnString = "INVALID";
                }

            }
            catch (Exception ex)
            {
                var functionName = string.Format("{0}.ConcatenateDate", ThisClassName);
                var msg = string.Format("DateString={0}", DateString);
                //Info.LogException(functionName, ex,msg);
                return "INVALID";
            }

            if (ReturnString != "INVALID")
            {
                DateTime FinalDate = DateTime.ParseExact(DateString, "MM/dd/yyyy", null);
                ReturnString = FinalDate.ToString(DateFormat);
            }

            return ReturnString;
        }

        public static string FormatDateRange(DateTime StartDate, DateTime EndDate, string FullDateFormat, string MonthDateFormat, string DayDateFormat, string PreferredFormat, string RangeDelim = " - ")
        {
            var finalDates = "";

            if (StartDate.Date == EndDate.Date)
            {
                //Dates are the same, return only 1
                switch (PreferredFormat)
                {
                    case "y":
                        finalDates = StartDate.ToString(FullDateFormat);
                        break;

                    case "M":
                        finalDates = StartDate.ToString(MonthDateFormat);
                        break;

                    case "d":
                        finalDates = StartDate.ToString(DayDateFormat);
                        break;

                    default:
                        finalDates = StartDate.ToString(FullDateFormat);
                        break;
                }

            }
            else
            {
                //Dates are different, combine into range
                if (StartDate.Year != EndDate.Year)
                {
                    //Different years
                    //Format with whole dates
                    finalDates = string.Format("{0}{1}{2}", StartDate.ToString(FullDateFormat), RangeDelim, EndDate.ToString(FullDateFormat));
                }
                else
                {
                    //Same year
                    if (StartDate.Month != EndDate.Month)
                    {
                        //Different months
                        var date1 = StartDate.ToString(MonthDateFormat);
                        var date2 = EndDate.ToString(MonthDateFormat);
                        finalDates = string.Format("{0}{1}{2}", date1, RangeDelim, date2);
                    }
                    else
                    {
                        //Same month
                        if (StartDate.Day != EndDate.Day)
                        {
                            //Different days
                            var date1 = StartDate.ToString(MonthDateFormat);
                            var date2 = EndDate.ToString(DayDateFormat);
                            finalDates = string.Format("{0}{1}{2}", date1, RangeDelim, date2);
                        }

                    }
                }
            }
            return finalDates;
        }

        public static bool IsValidDate(string DateStringToTest, string DateFormat)
        {
            bool ValidDate = false;
            try
            {
                var DateTest = DateTime.ParseExact(DateStringToTest, DateFormat, null);

                if (DateTest != null)
                {
                    ValidDate = true;
                }

            }
            catch (Exception exNonValidDate)
            {
                //Info.LogException("Functions.StringIsValidDate", exNonValidDate, "[DateStringToTest=" + DateStringToTest + "] [DateFormat=" + DateFormat + "] FALSE value returned. No action necessary");
                ValidDate = false;
            }

            return ValidDate;
        }

    
        public static string FuzzyDateFormat(this DateTime d)
        {
            //Based on:http://www.dotnetperls.com/pretty-date

            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // Don't allow out of range values.
            if (dayDiff < 0)
            {
                return "";
            }

            // Handle same-day times.
            if (dayDiff == 0)
            {
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }

                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }

                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }

                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }

                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }

            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago", dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double)dayDiff / 7));
            }
            if (dayDiff < 366)
            {
                return string.Format("{0} months ago",
                    Math.Ceiling((double)dayDiff / 30));
            }
            if (dayDiff > 365 & dayDiff < 730)
            {
                return "More than a year ago";
                //return string.Format("{0} years ago",
                //Math.Ceiling((double)dayDiff / 365));
            }
            //if (dayDiff > 365 & dayDiff < 730)
            //{
            return string.Format("More than {0} years ago",
                Math.Ceiling((double)dayDiff / 365));
            //}
        }

        public static string FuzzyDateFormat(this Nullable<DateTime> d)
        {
            if (d != null)
            {
                return ((DateTime)d).FuzzyDateFormat();
            }
            else
            {
                return "";
            }
        }


    }
}
