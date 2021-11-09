using AnarchyEngine.ECS.Components;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using AnarchyEngine.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering.Mesh {
    public class Mesh : IMesh {
        #region Cube Mesh

        private static readonly float[] _CubeVertsClean = {
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f, // 0  // X
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f, // 1
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f, // 2
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f, // 3
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f, // 4  // x
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f, // 5
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f, // 6
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f, // 7
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 8
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // 9
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 10
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // 11
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 12
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // 13
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 14
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // 15
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f, // 16
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f, // 17
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f, // 18
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f, // 19
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f, // 20
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f, // 21
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f, // 22
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f, // 23
        };

        private static readonly float[] _CubeVertices = {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f, // 0  // X
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f, // 1
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f, // 2
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f, // 2
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f, // 3
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f, // 0

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f, // 4  // x
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f, // 5
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f, // 6
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f, // 6
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f, // 7
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f, // 4

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 8
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // 9
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 10
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 10
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // 11
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 8

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 12
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // 13
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 14
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // 14
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // 15
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // 12

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f, // 16
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f, // 17
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f, // 18
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f, // 18
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f, // 19
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f, // 16

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f, // 20
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f, // 21
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f, // 22
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f, // 22
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f, // 23
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f  // 20
        };
        public static readonly Mesh Cube = FromRaw(_CubeVertices);
        #endregion
        public VertexProperty VertexProperties { get; set; }

        public readonly List<Vertex> Vertices = new List<Vertex>();

        public MeshFilter MeshFilter { get; set; }

        public Shader Shader { get; set; }

        public Texture Texture { get; set; }

        internal VertexArray VertexArray { get; private set; }



        public Mesh() : this(VertexProperty.All) { }

        public Mesh(VertexProperty properties) {
            VertexProperties = properties;
        }

        public void Init() {
            if (VertexArray != null) return;
            VertexArray = new VertexArray();
            VertexArray.SetStride(VertexProperties);
            //IEnumerable<float> raws;

            bool hasPosition = HasProperty(VertexProperty.Position),
                 hasNormal = HasProperty(VertexProperty.Normal),
                 hasUV = HasProperty(VertexProperty.UV);

            int offset = 0;
            int stride = (hasUV ? 2 : 0) +
                (hasPosition ? 3 : 0) +
                (hasNormal ? 3 : 0);
            if (hasPosition) {
                VertexArray.Model(Shader.PositionName, 3, stride, offset);
                offset += 3;
            }
            if (hasNormal) {
                VertexArray.Model(Shader.NormalName, 3, stride, offset);
                offset += 3;
            }
            if (hasUV) {
                VertexArray.Model(Shader.UVName, 2, stride, offset);
            }

            /*raws = Vertices.SelectMany(v => {
                var raw = new List<float>();
                if (hasPosition) raw.AddRange(v.Position.Raw);
                if (hasNormal) raw.AddRange(v.Normal.Raw);
                if (hasUV) raw.AddRange(v.UV.Raw);
                return raw;
            });*/

            VertexArray.AddVertexBuffer(Vertices);
            VertexArray.Init();
        }

        public void Render() {
            //Renderer.Instance.Submit();
            // VertexArray.Bind();
            // Shader?.SetVector3("color", Color4.DarkRed.ToVector3());
            if (Texture != null) {
                Texture.Use();
                Shader.SetInt("texture0", 0);
            }
            //////Renderer.Push("color", Color4.DarkRed.ToVector3());

            //Renderer.Instance.Submit(Shader, VertexArray);//, MeshFilter.Entity.Transform);
            //Texture?.Use();
            //Shader?.SetMatrix4("viewProjection", Camera.Main.ViewProjection);
            //Shader?.SetMatrix4("model", (Matrix4)MeshFilter.Entity.Transform);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public bool HasProperty(VertexProperty property) {
            return (VertexProperties & property) == property;
        }

        public static Mesh FromRaw(IEnumerable<float> raws, VertexProperty properties = VertexProperty.All) {
            Mesh mesh = new Mesh(properties);
            Queue<float> queue = new Queue<float>(raws);
            int stride = Vertex.Stride(properties);

            while (queue.Count > 0) {
                var vert = Vertex.FromRaw(queue.Pop(stride));
                mesh.Vertices.Add(vert);
            }
            
            return mesh;
        }
        public static Mesh LoadFbx(string filename, VertexProperty properties = VertexProperty.All) {
            var mesh = new Mesh(properties);
            
            mesh.Vertices.AddRange(FileHelper.LoadFbx(filename));

            return mesh;
        }
    }
}
