using System;

namespace Crawler.Mappers
{
    public class EnglishMonths
    {
        public const string January = "January";
        public const string February = "February";
        public const string March = "March";
        public const string April = "April";
        public const string May = "May";
        public const string June = "June";
        public const string July = "July";
        public const string August = "August";
        public const string September = "September";
        public const string October = "October";
        public const string November = "November";
        public const string December = "December";

        public static int Map(string Month)
        {
            switch (Month)
            {
                case January:
                    return 1;
                case February:
                    return 2;
                case March:
                    return 3;
                case April:
                    return 4;
                case May:
                    return 5;
                case June:
                    return 6;
                case July:
                    return 7;
                case August:
                    return 8;
                case September:
                    return 9;
                case October:
                    return 10;
                case November:
                    return 11;
                case December:
                    return 12;
                default:
                    throw new ArgumentException($"{Month} invalid month");
            }
        }
    }
}