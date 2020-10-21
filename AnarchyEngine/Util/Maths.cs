using AnarchyEngine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Util {
    public static class Maths {

        public const float Pi = (float)Math.PI;
        public const float HalfPi = Pi * .5f;
        public const float PiOver180 = Pi / 180f;

        public static readonly Random Rng = new Random(Time.EpochNow);

        public static int Clamp(int n, int min, int max) => Math.Max(min, Math.Min(n, max));
        public static float Clamp(float n, float min, float max) => Math.Max(min, Math.Min(n, max));

        public static bool WithinRange(Vector3 v, Vector3 u, float range = 1f) {
            return Vector3.DistanceSquared(v, u) <= range * range;
        }

        public static bool WithinRange(Vector2 v, Vector2 u, float range = 1f) {
            return Vector2.DistanceSquared(v, u) <= range * range;
        }

        // !Restructure
        public static Quaternion RotateTowards(Vector3 u, Vector3 v) {
            var q = new Quaternion {
                Xyz = Vector3.Cross(u, v)
            };

            var dot = Vector3.Dot(u, v);

            var w = Sqrt(u.LengthSquared * v.LengthSquared);

            q.W = w + dot;


            return q;
        }

        public static int RandInt(int max) => RandInt(0, max);

        public static int RandInt(int min, int max) => Rng.Next(min, max);

        public static float RandRange(float max) => RandRange(0, max);

        public static float RandRange(float min, float max) {
            return (float)(Rng.NextDouble() * (max - min)) + min;
        }

        public static float Max(params float[] vals) {
            float max = float.MinValue;

            foreach (var val in vals) {
                max = Math.Max(max, val);
            }

            return max;
        }

        public static float Min(params float[] vals) {
            float min = float.MaxValue;

            foreach (var val in vals) {
                min = Math.Min(min, val);
            }

            return min;
        }

        public static float Sin(float a) => (float)Math.Sin(a);

        public static float Cos(float a) => (float)Math.Cos(a);

        public static float Tan(float a) => (float)Math.Tan(a);

        public static float Sqrt(float a) => (float)Math.Sqrt(a);

        public static float Rad2Deg(float r) => r * (180f / Pi);

        public static float Deg2Rad(float d) => d * PiOver180;

        // May remove
        public static Vector3 CalculateFront(float pitch, float yaw) {
            return new Vector3(
                x: Cos(pitch) * Cos(yaw),
                y: Sin(pitch),
                z: Cos(pitch) * Sin(yaw));
        }
    }
}
