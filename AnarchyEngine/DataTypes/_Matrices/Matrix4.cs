using System;

namespace AnarchyEngine.DataTypes {
    public struct Matrix4 {
        public static readonly Matrix4 Identity = new Matrix4(
            Vector4.UnitX, Vector4.UnitY,
            Vector4.UnitZ, Vector4.UnitW);

        public Vector4 Row0, Row1, Row2, Row3;

        public float this[int r, int c] {
            get => Row(r)[c];
            set {
                switch (r) {
                    case 0: Row0[c] = value; break;
                    case 1: Row1[c] = value; break;
                    case 2: Row2[c] = value; break;
                    case 3: Row3[c] = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector4 Diagonal {
            get => new Vector4(M11, M22, M33, M44);
            set {
                M11 = value.X;
                M22 = value.Y;
                M33 = value.Z;
                M44 = value.W;
            }
        }

        public float Trace => M11 + M22 + M33 + M44;
        
        public Vector4 Column0 {
            get => new Vector4(M11, M21, M31, M41);
            set {
                M11 = value.X;
                M21 = value.Y;
                M31 = value.Z;
                M41 = value.W;
            }
        }
        public Vector4 Column1 {
            get => new Vector4(M12, M22, M32, M42);
            set {
                M12 = value.X;
                M22 = value.Y;
                M32 = value.Z;
                M42 = value.W;
            }
        }
        public Vector4 Column2 {
            get => new Vector4(M13, M23, M33, M43);
            set {
                M13 = value.X;
                M23 = value.Y;
                M33 = value.Z;
                M43 = value.W;
            }
        }
        public Vector4 Column3 {
            get => new Vector4(M14, M24, M34, M44);
            set {
                M14 = value.X;
                M24 = value.Y;
                M34 = value.Z;
                M44 = value.W;
            }
        }

        // M(row)(column)
        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }
        public float M13 { get => Row0.Z; set => Row0.Z = value; }
        public float M14 { get => Row0.W; set => Row0.W = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        public float M23 { get => Row1.Z; set => Row1.Z = value; }
        public float M24 { get => Row1.W; set => Row1.W = value; }

        public float M31 { get => Row2.X; set => Row2.X = value; }
        public float M32 { get => Row2.Y; set => Row2.Y = value; }
        public float M33 { get => Row2.Z; set => Row2.Z = value; }
        public float M34 { get => Row2.W; set => Row2.W = value; }

        public float M41 { get => Row3.X; set => Row3.X = value; }
        public float M42 { get => Row3.Y; set => Row3.Y = value; }
        public float M43 { get => Row3.Z; set => Row3.Z = value; }
        public float M44 { get => Row3.W; set => Row3.W = value; }

        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3) {
            Row0 = row0; Row1 = row1;
            Row2 = row2; Row3 = row3;
        }

        public Matrix4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33) {
            Row0 = new Vector4(m00, m01, m02, m03);
            Row1 = new Vector4(m10, m11, m12, m13);
            Row2 = new Vector4(m20, m21, m22, m23);
            Row3 = new Vector4(m30, m31, m32, m33);
        }

        public Matrix4(Matrix3 m00to33) {
            Row0 = m00to33.Row0.W0;
            Row1 = m00to33.Row1.W0;
            Row2 = m00to33.Row2.W0;
            Row3 = Vector4.Zero;
        }

        public Vector4 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
                case 2: return Row2;
                case 3: return Row3;
            }
            throw new IndexOutOfRangeException();
        }
        public void Row(int index, Vector4 row) => Row(index, ref row);
        public void Row(int index, ref Vector4 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                case 2: Row2 = row; break;
                case 3: Row3 = row; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public Vector4 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
                case 2: return Column2;
                case 3: return Column3;
            }
            throw new IndexOutOfRangeException();
        }
        public void Column(int index, Vector4 column) => Column(index, ref column);
        public void Column(int index, ref Vector4 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                case 2: Column2 = column; break;
                case 3: Column3 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public override bool Equals(object obj) {
            if (!(obj is Matrix4)) return false;
            var m = (Matrix4)obj;
            return Row0 == m.Row0 &&
                   Row1 == m.Row1 &&
                   Row2 == m.Row2 &&
                   Row3 == m.Row3;
        }
        public override string ToString() {
            return $"{Row0}\n{Row1}\n{Row2}\n{Row3}";
        }
        public override int GetHashCode() => base.GetHashCode();

        public static implicit operator OpenTK.Matrix4(Matrix4 m) => new OpenTK.Matrix4(m.Row0, m.Row1, m.Row2, m.Row3);
        public static implicit operator Matrix4(OpenTK.Matrix4 m) => new Matrix4(m.Row0, m.Row1, m.Row2, m.Row3);

        public static Matrix4 operator *(Matrix4 left, Matrix4 right) {
            Vector4 rightColumn0 = right.Column0,
                    rightColumn1 = right.Column1,
                    rightColumn2 = right.Column2,
                    rightColumn3 = right.Column3;

            return new Matrix4(
                m00: Vector4.Dot(left.Row0, rightColumn0),
                m01: Vector4.Dot(left.Row0, rightColumn1),
                m02: Vector4.Dot(left.Row0, rightColumn2),
                m03: Vector4.Dot(left.Row0, rightColumn3),

                m10: Vector4.Dot(left.Row1, rightColumn0),
                m11: Vector4.Dot(left.Row1, rightColumn1),
                m12: Vector4.Dot(left.Row1, rightColumn2),
                m13: Vector4.Dot(left.Row1, rightColumn3),

                m20: Vector4.Dot(left.Row2, rightColumn0),
                m21: Vector4.Dot(left.Row2, rightColumn1),
                m22: Vector4.Dot(left.Row2, rightColumn2),
                m23: Vector4.Dot(left.Row2, rightColumn3),

                m30: Vector4.Dot(left.Row3, rightColumn0),
                m31: Vector4.Dot(left.Row3, rightColumn1),
                m32: Vector4.Dot(left.Row3, rightColumn2),
                m33: Vector4.Dot(left.Row3, rightColumn3));
        }
        public static Matrix4 operator *(float scale, Matrix4 mat) {
            mat.Row0 *= scale;
            mat.Row1 *= scale;
            mat.Row2 *= scale;
            mat.Row3 *= scale;
            return mat;
        }
        public static Matrix4 operator *(Matrix4 mat, float scale) {
            mat.Row0 *= scale;
            mat.Row1 *= scale;
            mat.Row2 *= scale;
            mat.Row3 *= scale;
            return mat;
        }
    }
}
