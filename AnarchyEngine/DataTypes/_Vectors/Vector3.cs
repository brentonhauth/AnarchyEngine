using System;
using AnarchyEngine.Util;
using Jitter.LinearMath;
using Assimp;

namespace AnarchyEngine.DataTypes {
    public struct Vector3 {
        #region Static Readonly Vector3 Variables
        public static readonly Vector3 One = new Vector3(1f),
            Zero = new Vector3(0f),
            Max = new Vector3(float.MaxValue),
            Min = new Vector3(float.MinValue),
            PositiveInfinity = new Vector3(float.PositiveInfinity),
            NegativeInfinity = new Vector3(float.NegativeInfinity),
            UnitX = new Vector3(1, 0, 0),
            UnitY = new Vector3(0, 1, 0),
            UnitZ = new Vector3(0, 0, 1);
        #endregion

        public float X, Y, Z;

        #region Vector3 Properties & Indexers
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                }
                throw new IndexOutOfRangeException();
            } set {
                switch (index) {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public float Magnitude => Maths.Sqrt(MagnitudeSquared);
        public float MagnitudeSquared => (X * X) + (Y * Y) + (Z * Z);

        public Vector3 Normalized {
            get {
                float m = Magnitude;
                return m == 0 ? Zero : (this / m);
            }
        }
        public Vector2 Xy => new Vector2(X, Y);
        public Vector4 W0 => new Vector4(this, 0);
        public Vector4 W1 => new Vector4(this, 1);
        public float[] Raw => new[] { X, Y, Z };
        public float SumValues => X + Y + Z;
        #endregion

        #region Vector3 Constructors
        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(Vector2 xy, float z) {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }
        public Vector3(float xyz) {
            X = Y = Z = xyz;
        }
        #endregion

        #region Vector3 Methods
        public static Vector3 Random(float min, float max) {
            return new Vector3(
                x: Maths.RandRange(min, max),
                y: Maths.RandRange(min, max),
                z: Maths.RandRange(min, max));
        }

        public static float Distance(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            Vector3 AP = lineStart - point,
                AB = lineEnd - lineStart,
                proj = Projection(AP, AB); // Projection of AP on AB
            return (AB - proj).Magnitude;
        }
        public static float Distance(Vector3 u, Vector3 v) => (u - v).Magnitude;
        public static float DistanceSquared(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            Vector3 AP = lineStart - point,
                AB = lineEnd - lineStart,
                proj = Projection(AP, AB); // Projection of AP on AB
            return (AB - proj).MagnitudeSquared;
        }
        public static float DistanceSquared(Vector3 u, Vector3 v) => (u - v).MagnitudeSquared;

        public static float Dot(Vector3 u, Vector3 v) {
            Dot(ref u, ref v, out float result);
            return result;
        }
        public static void Dot(ref Vector3 u, ref Vector3 v, out float result) {
            result = (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);
        }

        public static Vector3 Cross(Vector3 u, Vector3 v) {
            return new Vector3(
                x: (u.Y * v.Z) - (u.Z * v.Y),
                y: (u.Z * v.X) - (u.X * v.Z),
                z: (u.X * v.Y) - (u.Y * v.X));
        }

        public static float Angle(Vector3 u, Vector3 v) {
            float magx = u.Magnitude * v.Magnitude;
            if (magx == 0) return 0f;
            double acos = Math.Acos(Dot(u, v) / magx);
            return (float)(180.0 / Math.PI * acos);
        }
        
        public Vector3 Projection(Vector3 against) => Projection(this, against);
        public static Vector3 Projection(Vector3 self, Vector3 against) {
            return Dot(against, self) / against.MagnitudeSquared * against;
        }


        public static void Add(in Vector3 left, in Vector3 right, out Vector3 sum) {
            sum.X = left.X + right.X;
            sum.Y = left.Y + right.Y;
            sum.Z = left.Z + right.Z;
        }

        public static void Subtract(in Vector3 left, in Vector3 right, out Vector3 diff) {
            diff.X = left.X - right.X;
            diff.Y = left.Y - right.Y;
            diff.Z = left.Z - right.Z;
        }

        public static void Multiply(in Vector3 left, in Vector3 right, out Vector3 prod) {
            prod.X = left.X * right.X;
            prod.Y = left.Y * right.Y;
            prod.Z = left.Z * right.Z;
        }
        public static void Multiply(in Vector3 vec, in float scale, out Vector3 prod) {
            prod.X = vec.X * scale;
            prod.Y = vec.Y * scale;
            prod.Z = vec.Z * scale;
        }

        public static void Divide(in Vector3 left, in float right, out Vector3 quot) {
            quot.X = left.X / right;
            quot.Y = left.Y / right;
            quot.Z = left.Z / right;
        }
        #endregion

        #region Vector3 Operators
        public static Vector3 operator *(Vector3 v, float s) {
            v.X *= s;
            v.Y *= s;
            v.Z *= s;
            return v;
        }
        public static Vector3 operator *(float s, Vector3 v) {
            v.X *= s;
            v.Y *= s;
            v.Z *= s;
            return v;
        }
        public static Vector3 operator /(Vector3 v, float s) {
            v.X /= s;
            v.Y /= s;
            v.Z /= s;
            return v;
        }
        public static Vector3 operator +(Vector3 left, Vector3 right) {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }
        public static Vector3 operator -(Vector3 left, Vector3 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }
        public static Vector3 operator -(Vector3 v) {
            v.X = -v.X;
            v.Y = -v.Y;
            v.Z = -v.Z;
            return v;
        }

        public static bool operator ==(Vector3 left, Vector3 right) {
            return left.X == right.X &&
                   left.Y == right.Y &&
                   left.Z == right.Z;
        }
        public static bool operator !=(Vector3 left, Vector3 right) {
            return left.X != right.X ||
                   left.Y != right.Y ||
                   left.Z != right.Z;
        }

        public static explicit operator Vector3(Vector2 v) => v.Z0;
        public static explicit operator Vector3(Vector4 v) => v.Xyz;

        public static implicit operator OpenTK.Vector3(Vector3 v) => new OpenTK.Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3(OpenTK.Vector3 v) => new Vector3(v.X, v.Y, v.Z);
        public static implicit operator JVector(Vector3 v) => new JVector(v.X, v.Y, v.Z);
        public static implicit operator Vector3(JVector v) => new Vector3(v.X, v.Y, v.Z);
        public static implicit operator Vector3D(Vector3 v) => new Vector3D(v.X, v.Y, v.Z);
        public static implicit operator Vector3(Vector3D v) => new Vector3(v.X, v.Y, v.Z);
        #endregion

        #region Vector3 Overrides
        public override string ToString() => $"({X}, {Y}, {Z})";
        public override bool Equals(object o) {
            return o is Vector3 && this == (Vector3)o;
        }
        public override int GetHashCode() {
            const int n = -1521134295;
            var code = -307843816;
            code = code * n + X.GetHashCode();
            code = code * n + Y.GetHashCode();
            code = code * n + Z.GetHashCode();
            return code;
        }
        #endregion
    }
}
