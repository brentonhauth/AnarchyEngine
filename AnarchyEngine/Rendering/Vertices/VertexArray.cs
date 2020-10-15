using System;
using System.Collections.Generic;
using System.Linq;
using AnarchyEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace AnarchyEngine.Rendering.Vertices {
    internal class VertexArray : IPipable {

        public int Handle { get; private set; }
        public VertexBuffer VertexBuffer { get; private set; }
        
        private List<VertexArrayDataModel> DataModels;

        private Shader Shader;

        // find better solution
        public int Count => (int)(VertexBuffer.Count / DataModels.First().Stride);

        public VertexArray(Shader shader) : this() {
            Shader = shader;
        }

        public VertexArray() {
            DataModels = new List<VertexArrayDataModel>();
        }

        public void AddVertexBuffer(VertexBuffer vb) {
            VertexBuffer = vb;
        }

        public void AddVertexBuffer(float[] data) {
            AddVertexBuffer(new VertexBuffer(data));
        }

        public void SetShader(Shader s) => Shader = s;

        public void Init() {
            if (VertexBuffer == null)
                AddVertexBuffer(new float[0]);

            VertexBuffer.Init();
            Handle = GL.GenVertexArray();
            Use();
            VertexBuffer.Use();
        }

        public void Bind(Shader shader) {
            foreach (var model in DataModels) {
                model.InitWithShader(shader);
            }
        }
        
        public void Model(string attr, int size, int stride, int offset) {
            var data = new VertexArrayDataModel(attr, size, stride, offset);
            DataModels.Add(data);
        }

        public void Use() => GL.BindVertexArray(Handle);

        public void Dispose() => GL.DeleteVertexArray(Handle);

        //public void AddData(IEnumerable<float> data) => Data.AddRange(data);
        // public void AddData(float data) => Data.Add(data);


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
