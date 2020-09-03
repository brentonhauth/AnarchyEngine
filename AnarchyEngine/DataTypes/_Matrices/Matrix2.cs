using System;

namespace AnarchyEngine.DataTypes {
    public struct Matrix2 {
        public static readonly Matrix2
            Identity = new Matrix2(Vector2.UnitX, Vector2.UnitY);

        public Vector2 Row0, Row1;

        public Vector2 Column0 {
            get => new Vector2(M11, M21);
            set {
                M11 = value.X;
                M21 = value.Y;
            }
        }
        public Vector2 Column1 {
            get => new Vector2(M12, M22);
            set {
                M12 = value.X;
                M22 = value.Y;
            }
        }

        public Matrix2 Transposed => new Matrix2(Column0, Column1);

        public Vector2 Diagonal {
            get => new Vector2(M11, M22);
            set {
                M11 = value.X;
                M22 = value.Y;
            }
        }

        public float Trace => M11 + M22;

        public float M11 { get => Row0.X; set => Row0.X = value; }
        public float M12 { get => Row0.Y; set => Row0.Y = value; }

        public float M21 { get => Row1.X; set => Row1.X = value; }
        public float M22 { get => Row1.Y; set => Row1.Y = value; }

        public Matrix2(Vector2 row0, Vector2 row1) {
            Row0 = row0;
            Row1 = row1;
        }

        public Matrix2(
            float m00, float m01,
            float m10, float m11) {
            Row0 = new Vector2(m00, m01);
            Row1 = new Vector2(m10, m11);
        }

        public Vector2 Row(int index) {
            switch (index) {
                case 0: return Row0;
                case 1: return Row1;
            }
            throw new IndexOutOfRangeException();
        }
        public void Row(int index, Vector2 row) => Row(index, ref row);
        public void Row(int index, ref Vector2 row) {
            switch (index) {
                case 0: Row0 = row; break;
                case 1: Row1 = row; break;
                default: throw new IndexOutOfRangeException();
            }
        }
        public Vector2 Column(int index) {
            switch (index) {
                case 0: return Column0;
                case 1: return Column1;
            }
            throw new IndexOutOfRangeException();
        }
        public void Column(int index, Vector2 column) => Column(index, ref column);
        public void Column(int index, ref Vector2 column) {
            switch (index) {
                case 0: Column0 = column; break;
                case 1: Column1 = column; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public static implicit operator OpenTK.Matrix2(Matrix2 m) => new OpenTK.Matrix2(m.Row0, m.Row1);
        public static implicit operator Matrix2(OpenTK.Matrix2 m) => new Matrix2(m.Row0, m.Row1);
    }
}
