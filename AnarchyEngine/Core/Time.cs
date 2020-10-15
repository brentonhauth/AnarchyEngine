using System;

namespace AnarchyEngine.Core {
    public static class Time {
        private static readonly DateTime EpochStart = new DateTime(1970, 1, 1);

        public static int EpochNow => Epoch(DateTime.UtcNow);

        public static int Epoch(this DateTime time) => (int)(time - EpochStart).TotalSeconds;

        public static DateTime FromEpoch(int timestamp) => EpochStart.AddSeconds(timestamp);

        public static float DeltaTime { get; private set; }

        public static float TotalTime { get; private set; } = 0;

        public static ulong Ticks { get; private set; } = 0;

        internal static void Update(in double time) {
            DeltaTime = (float)time;
            TotalTime += DeltaTime;
            Ticks++;
        }
    }
}
