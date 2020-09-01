using System;

namespace AnarchyEngine.Core {
    public static class Time {
        private static readonly DateTime EpochStart = new DateTime(1970, 1, 1);

        public static int EpochNow => Epoch(DateTime.UtcNow);

        public static int Epoch(this DateTime time) => (int)(time - EpochStart).TotalSeconds;

        public static DateTime FromEpoch(int timestamp) => EpochStart.AddSeconds(timestamp);

        public static double DeltaTime { get; internal set; }

        public static decimal TotalTime { get; internal set; } = 0;
    }
}
