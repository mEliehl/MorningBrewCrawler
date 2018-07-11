using System;

namespace Crawler.Mappers
{
    public class EnglishMonths
    {
        private const string January = "January";
        private const string February = "February";
        private const string March = "March";
        private const string April = "April";
        private const string May = "May";
        private const string June = "June";
        private const string July = "July";
        private const string August = "August";
        private const string September = "September";
        private const string October = "October";
        private const string November = "November";
        private const string December = "December";

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