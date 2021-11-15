using System;
using System.Collections.Generic;
using Jitter.LinearMath;

namespace AnarchyEngine.DataTypes {

    public struct Matrix3 {
        public static readonly Matrix3 Identity = new Matrix3(
            Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public Vector3 Row0, Row1, Row2;

        public float this[int r, int c] {
            get => Row(r)[c];
            set {
                switch (r) {
                    case 0: Row0[c] = value; break;
                    case 1: Row1[c] = value; break;
                    case 2: Row2[c] = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Matrix3 Transposed => new Matrix3(Column0, Column1, Column2);

        public Vector3 Diagonal {
            get => new Vector3(M11, M22, M33);
            set {
                M11 = value.X;
                M22 = value.Y;
                M33 = value.Z;
            }
        }
        
        public float Trace => M11 + M22 + M33;


        public Vector3 Column0 {
            get => new Vector3(M11, M21, M31);
            set {
                M11 = value.X;
                M21 = value.Y;
                M31 = value.Z;
            }
        }
        public Vector3 Column1 {
            get => new Vector3(M12, M22, M32);
            set {
                M12 = value.X;
                M22 = value.Y;
                M32 = value.Z;
            }
        }
        public Vector3 Column2 {
            get => new Vector3(M13, M23, M33);
            set {
                M13 = value.X;
                M23 = value.Y;
                M33 = value.Z;
            }
        }

        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }
        public float M13 { get => Row0.Z; set => Row0.Z = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }
        public float M23 { get => Row1.Z; set => Row1.Z = value; }

        public float M31 { get => Row2.X; set => Row2.X = value; }
        public float M32 { get => Row2.Y; set => Row2.Y = value; }
        public float M33 { get => Row2.Z; set => Row2.Z = value; }

        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2) {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }

        public Matrix3(
            float m00, float m01, float m02,
            float m10, float m11, float m12,
            float m20, float m21, float m22) {
            Row0 = new Vector3(m00, m01, m02);
            Row1 = new Vector3(m10, m11, m12);
            Row2 = new Vector3(m20, m21, m22);
        }

        public Matrix3(Matrix2 m00to22) {
            Row0 = m00to22.Row0.Z0;
            Row1 = m00to22.Row1.Z0;
            Row2 = Vector3.UnitZ;
        }

        public Vector3 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
                case 2: return Row2;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Row(int index, Vector3 row) => Row(index, ref row);
        public void Row(int index, ref Vector3 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                case 2: Row2 = row; break;
                default: throw new IndexOutOfRangeException();
            }

        }

        public Vector3 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
                case 2: return Column2;
                default: throw new IndexOutOfRangeException();
            }
        }
        public void Column(int index, Vector3 column) => Column(index, ref column);
        public void Column(int index, ref Vector3 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                case 2: Column2 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }
        
        public static bool operator ==(Matrix3 left, Matrix3 right) {
            return left.Row0 == right.Row0 &&
                   left.Row1 == right.Row1 &&
                   left.Row2 == right.Row2;
        }

        public static bool operator !=(Matrix3 left, Matrix3 right) {
            return left.Row0 != right.Row0 ||
                   left.Row1 != right.Row1 ||
                   left.Row2 != right.Row2;
        }

        public static explicit operator Matrix3(Matrix4 m) => new Matrix3(m.Row0.Xyz, m.Row1.Xyz, m.Row2.Xyz);
        public static implicit operator JMatrix(Matrix3 m) => new JMatrix(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33);
        public static implicit operator Matrix3(JMatrix m) => new Matrix3(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33);
        public static implicit operator OpenTK.Matrix3(Matrix3 m) => new OpenTK.Matrix3(m.Row0, m.Row1, m.Row2);
        public static implicit operator Matrix3(OpenTK.Matrix3 m) => new Matrix3(m.Row0, m.Row1, m.Row2);

        public override bool Equals(object obj) {
            if (!(obj is Matrix3)) {
                return false;
            }

            var matrix = (Matrix3)obj;
            return EqualityComparer<Vector3>.Default.Equals(Row0, matrix.Row0) &&
                   EqualityComparer<Vector3>.Default.Equals(Row1, matrix.Row1) &&
                   EqualityComparer<Vector3>.Default.Equals(Row2, matrix.Row2);
        }

        public override int GetHashCode() {
            const int n = -1521134295;
            var code = -99944038;
            code = code * n + Row0.GetHashCode();
            code = code * n + Row1.GetHashCode();
            code = code * n + Row2.GetHashCode();
            return code;
        }
    }
}
