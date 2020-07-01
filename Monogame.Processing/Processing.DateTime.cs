using System;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Time & Date

        public int second() => DateTime.Now.Second;

        public int minute() => DateTime.Now.Minute;

        public int hour() => DateTime.Now.Hour;

        public int day() => DateTime.Now.Day;

        public int month() => DateTime.Now.Month;

        public int year() => DateTime.Now.Year;

        public int millis() => (int)_time.ElapsedMilliseconds;

        #endregion
    }
}
