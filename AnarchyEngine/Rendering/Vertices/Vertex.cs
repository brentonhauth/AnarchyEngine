﻿using AnarchyEngine.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering.Vertices {
    [Flags]
    public enum VertexProperty {
        None = 0,
        Position = 1,
        Normal = 2,
        UV = 4,
        All = Position | Normal | UV
    }

    public struct Vertex : IEquatable<Vertex> /*MIConvexHull.IVertex*/ {

        public static int Stride(VertexProperty p) {
            return ((p & VertexProperty.UV) == VertexProperty.UV ? 2 : 0) +
                ((p & VertexProperty.Position) == VertexProperty.Position ? 3 : 0) +
                ((p & VertexProperty.Normal) == VertexProperty.Normal ? 3 : 0);
        }

        public Vector3 Position;

        public Vector3 Normal;

        public Vector2 UV;

        public float[] Raw => new[] {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z,
            UV.X, UV.Y
        };

        /*double[] MIConvexHull.IVertex.Position => new double[3] {
            Position.X, Position.Y, Position.Z
        };*/

        public Vertex(Vector3 position, Vector3 normal, Vector2 uv) {
            Position = position;
            Normal = normal;
            UV = uv;
        }

        public Vertex(Vector3 position, Vector3 normal) : this(position, normal, Vector2.Zero) { }

        public Vertex(Vector3 position) : this(position, Vector3.Zero) { }

        public float[] OnlyRaw(VertexProperty props) => OnlyRaw(Stride(props));

        public float[] OnlyRaw(int stride) {
            var raw = new float[stride];
            int i = 0;

            if (stride >= 3) {
                raw[i++] = Position.X;
                raw[i++] = Position.Y;
                raw[i++] = Position.Z;
            }
            if (stride >= 6) {
                raw[i++] = Normal.X;
                raw[i++] = Normal.Y;
                raw[i++] = Normal.Z;
            }
            if (stride >= 8) {
                raw[i++] = UV.X;
                raw[i++] = UV.Y;
            }

            return raw;
        }

        


        public Vertex[] FromPositions(float[] positions) {
            List<Vertex> vertices = new List<Vertex>();
            int i = 0;

            foreach (var pos in positions) {
                var v = new Vector3(
                    x: positions[i++],
                    y: positions[i++],
                    z: positions[i++]);

                vertices.Add(new Vertex(v));
            }

            return vertices.ToArray();
        }

        public static Vertex FromRaw(float[] points) {
            Vertex vertex = new Vertex();

            if (points.Length >= 3) {
                vertex.Position = new Vector3(points[0], points[1], points[2]);
            }

            if (points.Length >= 6) {
                vertex.Normal = new Vector3(points[3], points[4], points[5]);
            }

            if (points.Length >= 8) {
                vertex.UV = new Vector2(points[6], points[7]);
            }

            return vertex;
        }

        public static bool operator ==(Vertex left, Vertex right) {
            return left.Position == right.Position &&
                   left.Normal == right.Normal &&
                   left.UV == right.UV;
        }

        public static bool operator !=(Vertex left, Vertex right) {
            return left.Position != right.Position ||
                   left.Normal != right.Normal ||
                   left.UV != right.UV;
        }

        public bool Equals(Vertex v) => QuickEquals(v);

        public bool QuickEquals(Vertex v) => Position == v.Position;

        public override bool Equals(object o) {
            return o is Vertex v && this == v;
        }

        public override int GetHashCode() {
            const int n = -1521134295;
            var code = 2128464926;
            code = code * n + Position.GetHashCode();
            code = code * n + Normal.GetHashCode();
            code = code * n + UV.GetHashCode();
            return code;
        }

        public override string ToString() {
            return $"Position: {Position}, Normal: {Normal}, UV: {UV}";
        }
    }
}
