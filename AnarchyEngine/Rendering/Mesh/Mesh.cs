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
        private static readonly float[] _CubeVertices = {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };
        public static readonly Mesh Cube = FromRaw(_CubeVertices).AddShaderWithVA(Shader.Default);
        #endregion
        public VertexProperty VertexProperties { get; set; }

        public readonly List<Vertex> Vertices = new List<Vertex>();

        public MeshFilter MeshFilter { get; set; }

        public Shader Shader { get; set; }

        public Texture Texture { get; set; }

        public VertexArray VertexArray { get; private set; }



        public Mesh() : this(VertexProperty.All) { }

        public Mesh(VertexProperty properties) {
            VertexProperties = properties;
        }

        public Mesh AddShader(Shader shader) {
            Shader = shader;
            return this;
        }

        public Mesh AddShaderWithVA(Shader shader) {
            Shader = shader;
            VertexArray = new VertexArray(Shader);
            return this;
        }

        public Mesh AddTexture(Texture texture) {
            Texture = texture;
            return this;
        }

        public Mesh AddVertexArray(VertexArray vertexArray) {
            VertexArray = vertexArray;
            return this;
        }

        public void Init() {
            Texture?.Init();
            Shader.Init();
            IEnumerable<float> raws;

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

            raws = Vertices.SelectMany(v => {
                var raw = new List<float>();
                if (hasPosition) raw.AddRange(v.Position.Raw);
                if (hasNormal) raw.AddRange(v.Normal.Raw);
                if (hasUV) raw.AddRange(v.UV.Raw);
                return raw;
            });

            VertexArray.AddData(raws);
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
            //Shader?.SetMatrix4("viewProjection", World.MainCamera.ViewProjection);
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

            mesh.AddShaderWithVA(Shader.Default);

            mesh.Vertices.AddRange(FileHelper.LoadFbx(filename));

            return mesh;
        }
    }
}
