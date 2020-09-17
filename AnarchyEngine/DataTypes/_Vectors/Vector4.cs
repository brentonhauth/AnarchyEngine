using System;
using AnarchyEngine.Util;

namespace AnarchyEngine.DataTypes {
    public struct Vector4 {
        #region Static Readonly Vector4 Variables
        public static readonly Vector4 One = new Vector4(1f),
            Zero = new Vector4(0f),
            Max = new Vector4(float.MaxValue),
            Min = new Vector4(float.MinValue),
            Infinity = new Vector4(float.PositiveInfinity),
            NegativeInfinity = new Vector4(float.NegativeInfinity),
            UnitX = new Vector4(1, 0, 0, 0),
            UnitY = new Vector4(0, 1, 0, 0),
            UnitZ = new Vector4(0, 0, 1, 0),
            UnitW = new Vector4(0, 0, 0, 1);
        #endregion

        public float X, Y, Z, W;

        #region Vector4 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                }
                throw new IndexOutOfRangeException();
            }
            set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public float Magnitude => Maths.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y) + (Z * Z) + (W * W);

        public Vector4 Normalized {
            get {
                float m = Magnitude;
                return m == 0 ? Zero : (this / m);
            }
        }
        public Vector2 Xy => new Vector2(X, Y);
        public Vector3 Xyz => new Vector3(X, Y, Z);
        public float[] Raw => new[] { X, Y, Z, W };
        public float SumValues => X + Y + Z + W;
        #endregion

        #region Vector4 Constructors
        public Vector4(float x, float y, float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4(Vector2 xy, float z, float w) {
            X = xy.X; Y = xy.Y; Z = z; W = w;
        }
        public Vector4(Vector2 xy, Vector2 zw) {
            X = xy.X; Y = xy.Y;
            Z = zw.X; W = zw.Y;
        }
        public Vector4(Vector3 xyz, float w) {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }
        public Vector4(float xyzw) {
            X = Y = Z = W = xyzw;
        }
        #endregion

        #region Vector4 Methods
        public static float Distance(Vector4 point, Vector4 lineStart, Vector4 lineEnd) {
            Vector4 AP = lineStart - point,
                AB = lineEnd - lineStart;

            var proj = Projection(AP, AB); // Projection of AP on AB

            return (AB - proj).Magnitude;
        }
        public static float Distance(Vector4 u, Vector4 v) => (u - v).Magnitude;
        public static float DistanceSquared(Vector4 point, Vector4 lineStart, Vector4 lineEnd) {
            Vector4 AP = lineStart - point,
                AB = lineEnd - lineStart,
                proj = Projection(AP, AB); // Projection of AP on AB
            return (AB - proj).MagnitudeSquared;
        }
        public static float DistanceSquared(Vector4 u, Vector4 v) => (u - v).MagnitudeSquared;

        public static float Dot(Vector4 u, Vector4 v) {
            return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z) + (u.W * v.W);
        }

        public static float Angle(Vector4 u, Vector4 v) {
            float umag = u.Magnitude;
            if (umag == 0) return 0f;

            float vmag = v.Magnitude;
            if (vmag == 0) return 0f;

            umag *= vmag;
            double acos = Math.Acos(Dot(u, v) / umag);
            return (float)(180.0 / Math.PI * acos);
        }

        public Vector4 Projection(Vector4 against) => Projection(this, against);
        public static Vector4 Projection(Vector4 self, Vector4 against) {
            return Dot(against, self) / against.MagnitudeSquared * against;
        }
        #endregion

        #region Vector4 Operators
        public static Vector4 operator *(Vector4 left, Vector4 right) {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            left.W *= right.W;
            return left;
        }
        public static Vector4 operator *(Vector4 v, float s) {
            v.X *= s;
            v.Y *= s;
            v.Z *= s;
            v.W *= s;
            return v;
        }
        public static Vector4 operator *(float s, Vector4 v) {
            v.X *= s;
            v.Y *= s;
            v.Z *= s;
            v.W *= s;
            return v;
        }
        public static Vector4 operator /(Vector4 v, float s) {
            v.X /= s;
            v.Y /= s;
            v.Z /= s;
            v.W /= s;
            return v;
        }
        public static Vector4 operator +(Vector4 left, Vector4 right) {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }
        public static Vector4 operator -(Vector4 left, Vector4 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }
        public static Vector4 operator -(Vector4 v) {
            v.X = -v.X;
            v.Y = -v.Y;
            v.Z = -v.Z;
            v.W = -v.W;
            return v;
        }
        public static bool operator ==(Vector4 left, Vector4 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z &&
                   left.W == right.W;
        }
        public static bool operator !=(Vector4 left, Vector4 right) {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z ||
                   left.W != right.W;
        }

        public static explicit operator Vector4(Vector2 v) => new Vector4(v, Vector2.Zero);
        public static explicit operator Vector4(Vector3 v) => v.W0;

        public static implicit operator OpenTK.Vector4(Vector4 v) => new OpenTK.Vector4(v.X, v.Y, v.Z, v.W);
        public static implicit operator Vector4(OpenTK.Vector4 v) => new Vector4(v.X, v.Y, v.Z, v.W);
        #endregion

        #region Vector4 Overrides
        public override string ToString() {
            return $"({X}, {Y}, {Z}, {W})";
        }
        public override bool Equals(object o) {
            return (o is Vector4) && this == (Vector4)o;
        }
        public override int GetHashCode() {
            const int n = -1521134295;
            int code = 707706286;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            code = code * n + Z.GetHashCode();
            code = code * n + W.GetHashCode();
            return code;
        }
        #endregion
    }
}
