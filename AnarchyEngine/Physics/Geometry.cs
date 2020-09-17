using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using AnarchyEngine.Rendering.Vertices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Physics {
    public class Geometry : IEnumerable<Vertex> {
        public readonly List<Vertex> Points;

        public Matrix4 Model { get; private set; }

        public Geometry(IEnumerable<Vertex> points) {
            Points = new List<Vertex>(points);
            Model = Matrix4.Identity;
        }

        public Geometry() {
            Points = new List<Vertex>();
            Model = Matrix4.Identity;
        }

        public void AddModel(Transform transform) {
            Model = (Matrix4)transform;
        }


        /*public static Geometry FromMesh(Mesh mesh) {
            var result = ConvexHull.Create(mesh.Vertices);

            var hull = result.Result;

            var verts = hull.Faces.SelectMany(f => f.Vertices);

            return new Geometry(verts);
        }*/

        public IEnumerator<Vertex> GetEnumerator() {
            return ((IEnumerable<Vertex>)Points).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable<Vertex>)Points).GetEnumerator();
        }
    }
}
