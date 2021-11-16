using System;
using AnarchyEngine.Util;
using Assimp;

namespace AnarchyEngine.DataTypes {
    public struct Vector2 : IEquatable<Vector2> {
        #region Static Readonly Vector2 Variables
        public static readonly Vector2 One = new Vector2(1f),
            Zero = new Vector2(0f),
            Max = new Vector2(float.MaxValue),
            Min = new Vector2(float.MinValue),
            Infinity = new Vector2(float.PositiveInfinity),
            NegativeInfinity = new Vector2(float.NegativeInfinity),
            UnitX = new Vector2(1, 0),
            UnitY = new Vector2(0, 1);
        #endregion

        public float X, Y;

        #region Vector2 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                }
                throw new IndexOutOfRangeException();
            } set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
        
        public float Magnitude => Maths.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y);
        public Vector2 Normalized {
            get {
                float m = Magnitude;
                return m == 0 ? Zero : (this / m);
            }
        }
        public Vector3 Z0 => new Vector3(this, 0);
        public Vector3 Z1 => new Vector3(this, 1);
        public float[] Raw => new[] { X, Y };
        public float SumValues => X + Y;
        #endregion

        #region Vector2 Constructors
        public Vector2(float x, float y) {
            X = x; Y = y;
        }
        public Vector2(float xy) {
            X = Y = xy;
        }
        #endregion

        #region Vector2 Methods
        public static float Distance(Vector2 point, Vector2 lineStart, Vector2 lineEnd) {
            Vector2 AP = lineStart - point,
                AB = lineEnd - lineStart;

            var proj = Projection(AP, AB); // Projection of AP on AB

            return (AB - proj).Magnitude;
        }
        public static float Distance(Vector2 a, Vector2 b) => (a - b).Magnitude;
        public static float DistanceSquared(Vector2 point, Vector2 lineStart, Vector2 lineEnd) {
            Vector2 AP = lineStart - point,
                AB = lineEnd - lineStart,
                proj = Projection(AP, AB); // Projection of AP on AB
            return (AB - proj).MagnitudeSquared;
        }
        public static float DistanceSquared(Vector2 a, Vector2 b) => (a - b).MagnitudeSquared;

        public static float Dot(Vector2 u, Vector2 v) {
            Dot(in u, in v, out float result);
            return result;
        }

        public static void Dot(in Vector2 u, in Vector2 v, out float result) {
            result = (u.X * v.X) + (u.Y * v.Y);
        }

        public static float Angle(Vector2 u, Vector2 v) {
            float magx = u.Magnitude * v.Magnitude;
            if (magx == 0) return 0f;
            double acos = Math.Acos(Dot(u, v) / magx);
            return Maths.Rad2Deg((float)acos);
        }

        public Vector2 Projection(Vector2 against) => Projection(this, against);
        public static Vector2 Projection(Vector2 self, Vector2 against) {
            return Dot(against, self) / against.MagnitudeSquared * against;
        }

        public static void Add(in Vector2 left, in Vector2 right, out Vector2 sum) {
            sum.X = left.X + right.X;
            sum.Y = left.Y + right.Y;
        }

        public static void Subtract(in Vector2 left, in Vector2 right, out Vector2 diff) {
            diff.X = left.X - right.X;
            diff.Y = left.Y - right.Y;
        }

        public static void Multiply(in Vector2 left, in Vector2 right, out Vector2 prod) {
            prod.X = left.X * right.X;
            prod.Y = left.Y * right.Y;
        }
        public static void Multiply(in Vector2 vec, in float scale, out Vector2 prod) {
            prod.X = vec.X * scale;
            prod.Y = vec.Y * scale;
        }

        public static void Divide(in Vector2 left, in float right, out Vector2 quot) {
            quot.X = left.X / right;
            quot.Y = left.Y / right;
        }
        #endregion

        #region Vector2 Operators
        public static Vector2 operator *(Vector2 v, float s) {
            v.X *= s;
            v.Y *= s;
            return v;
        }
        public static Vector2 operator *(float s, Vector2 v) {
            v.X *= s;
            v.Y *= s;
            return v;
        }
        public static Vector2 operator /(Vector2 v, float s) {
            v.X /= s;
            v.Y /= s;
            return v;
        }
        public static Vector2 operator +(Vector2 left, Vector2 right) {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }
        public static Vector2 operator -(Vector2 left, Vector2 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }
        public static Vector2 operator -(Vector2 v) {
            v.X = -v.X;
            v.Y = -v.Y;
            return v;
        }

        public static bool operator ==(Vector2 left, Vector2 right) {
            return left.X == right.X &&
                   left.Y == right.Y;
        }
        public static bool operator !=(Vector2 left, Vector2 right) {
            return left.X != right.X ||
                   left.Y != right.Y;
        }

        public static explicit operator Vector2(Vector3 v) => v.Xy;
        public static explicit operator Vector2(Vector4 v) => v.Xy;

        public static implicit operator OpenTK.Vector2(Vector2 v) => new OpenTK.Vector2(v.X, v.Y);
        public static implicit operator Vector2(OpenTK.Vector2 v) => new Vector2(v.X, v.Y);
        public static implicit operator Vector2D(Vector2 v) => new Vector2D(v.X, v.Y);
        public static implicit operator Vector2(Vector2D v) => new Vector2(v.X, v.Y);
        #endregion

        #region Vector2 Overrides
        public override string ToString() => $"({X}, {Y})";
        public override bool Equals(object o) {
            return o is Vector2 v && Equals(v);
        }
        public bool Equals(Vector2 other) {
            return X == other.X && Y == other.Y;
        }
        public override int GetHashCode() {
            const int n = -1521134295;
            int code = 1861411795;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            return code;
        }
        #endregion
    }
}
