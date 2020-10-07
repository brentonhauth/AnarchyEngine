using System;
using System.Collections.Generic;
using System.Linq;
using AnarchyEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;


namespace AnarchyEngine.Rendering.Vertices {
    public class VertexArray : IDisposable {

        public int VAO { get; private set; }
        public int VBO { get; private set; }

        private List<float> Data;

        private List<VertexArrayDataModel> DataModels;

        private Shader Shader;

        // find better solution
        public int Count => (int)(Data.Count / DataModels.First().Stride);

        public VertexArray(Shader shader) : this() {
            Shader = shader;
        }

        public VertexArray() {
            DataModels = new List<VertexArrayDataModel>();
            Data = new List<float>();
        }

        public void SetShader(Shader s) => Shader = s;

        public void Init() {
            int vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: Data.Count * sizeof(float),
                data: Data.ToArray(),
                usage: BufferUsageHint.StaticDraw);

            Init(vbo);
        }

        public void Init(int vbo) {

            VBO = vbo;
            VAO = GL.GenVertexArray();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            
            // Init With Shader
        }

        public void InitWithShader(Shader shader) {
            foreach (var model in DataModels) {
                model.InitWithShader(shader);
            }
        }
        
        public void Model(string attr, int size, int stride, int offset) {
            var data = new VertexArrayDataModel(attr, size, stride, offset);
            DataModels.Add(data);
        }

        public void Bind() => GL.BindVertexArray(VAO);

        public void Dispose() {
            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);
            GL.DeleteProgram(Shader.Handle);
        }

        public void AddData(IEnumerable<float> data) => Data.AddRange(data);
        public void AddData(float data) => Data.Add(data);

        //--------------------------------------------------------------------------------

        struct VertexArrayDataModel {
            public readonly string Attr;
            public readonly int Size, Stride, Offset;

            public VertexArrayDataModel(string attr, int size, int stride, int offset) {
                Attr = attr;
                Size = size;
                Stride = stride;
                Offset = offset;
            }

            public void InitWithShader(Shader shader) {
                int location = shader.GetAttribLocation(Attr);
                GL.EnableVertexAttribArray(location);
                GL.VertexAttribPointer(
                    index: location,
                    size: Size,
                    type: VertexAttribPointerType.Float,
                    normalized: false,
                    stride: Stride * sizeof(float),
                    offset: Offset * sizeof(float));
            }
        }
    }
}
