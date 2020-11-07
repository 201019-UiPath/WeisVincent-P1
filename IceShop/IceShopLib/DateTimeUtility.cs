using System;

namespace IceShopLib
{
    /// <summary>
    /// This class handles date-time logic, designed for use with order entries and history storage, for conversions to and from a Database friendly number
    /// </summary>
    public static class DateTimeUtility
    {
        private static readonly DateTime EpochTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double GetUnixEpochAsDouble(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() - EpochTime1970;

            return unixTime.TotalSeconds;
        }

        public static DateTime GetDateTimeFromUnixEpochAsDouble(this double dateTimeAsDoublePOSIX)
        {

            DateTime convertedTime = (EpochTime1970 + TimeSpan.FromSeconds(dateTimeAsDoublePOSIX));


            return convertedTime;
        }

        public static long GetUnixEpochAsTicks(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() - EpochTime1970;

            return unixTime.Ticks;
        }

    }
}
