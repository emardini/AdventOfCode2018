using System;

namespace Day_4_A
{
    public class DataEntry
    {
        public int? GuardId { get; internal set; }
        public DateTime StartTime { get; internal set; }
        public DateTime EndTime { get; internal set; }

        public int ElapsedMinutesSleep => (int)(EndTime - StartTime).TotalMinutes;
    }
}