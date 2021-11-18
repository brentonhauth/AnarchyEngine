using System;
using System.Collections.Generic;
using System.Linq;
using AnarchyEngine.Core;
using AnarchyEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;

namespace AnarchyEngine.Rendering.Vertices {
    internal class VertexArray : IPipable {

        private bool _Disposed = false;

        public int Handle { get; protected set; } = 0;
        public int Stride { get; private set; } = 8;
        public VertexBuffer VertexBuffer { get; private set; }

        private static int CurrentlyInUse = 0;
        
        public bool Initialized { get; private set; }

        private List<VertexArrayDataModel> DataModels;
        
        // find better solution
        public int Count => (int)(VertexBuffer.Count / Stride);

        public VertexArray() {
            DataModels = new List<VertexArrayDataModel>();

        }

        ~VertexArray() {
            if (!_Disposed) Dispose();
        }

        public void SetStride(VertexProperty props) {
            Stride = Vertex.Stride(props);
        }

        public void AddVertexBuffer(VertexBuffer vb) {
            VertexBuffer = vb;
        }

        public virtual void AddVertexBuffer(float[] data) {
            AddVertexBuffer(new VertexBuffer(data));
        }

        public virtual void AddVertexBuffer(IEnumerable<Vertex> data) {
            float[] buff = new float[Stride],
                raws = new float[data.Count() * Stride];

            int i = 0;
            foreach (Vertex v in data) {
                v.OnlyRawNonAloc(buff);
                buff.CopyTo(raws, i);
                i += Stride;
            }

            AddVertexBuffer(raws);
        }

        public virtual void Init() {
            if (VertexBuffer == null)
                throw new Exception("No VertexBuffer");
            //AddVertexBuffer(new float[0]);
            Initialized = true;
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

        public void Use() {
            if (Handle != CurrentlyInUse) {
                GL.BindVertexArray(CurrentlyInUse = Handle);
            }
        }

        public virtual void Draw() {
            
        }

        public virtual void Dispose() {
            VertexBuffer.Dispose();
            //GL.DeleteVertexArray(Handle);
            _Disposed = true;
        }

        //public void AddData(IEnumerable<float> data) => Data.AddRange(data);
        // public void AddData(float data) => Data.Add(data);


        //--------------------------------------------------------------------------------
        #region VertexArray Data Model
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
        #endregion
    }

    internal class ElementVertexArray : VertexArray {
        public ElementBuffer ElementBuffer { get; private set; }

        public ElementVertexArray() : base() { }
        
        public override void AddVertexBuffer(IEnumerable<Vertex> data) {
            var verts = new List<Vertex>();

            uint[] indices = data.Select(v => {
                if (!verts.Contains(v)) verts.Add(v);
                return (uint)verts.IndexOf(v);
            }).ToArray();

            ElementBuffer = new ElementBuffer(indices);
            base.AddVertexBuffer(verts);
        }

        public override void Init() {
            if (ElementBuffer == null)
                throw new Exception("No ElementBuffer");
            VertexBuffer.Init();//+
            ElementBuffer.Init(); // ! May have to init VB before EB

            // base.Init();
            Handle = GL.GenVertexArray();//+
            Use();//+
            VertexBuffer.Use();
            ElementBuffer.Use();
        }

        public override void Draw() {
            GL.DrawElements(PrimitiveType.Triangles, ElementBuffer.Count, DrawElementsType.UnsignedInt, 0);
        }

        public override void Dispose() {
            ElementBuffer.Dispose();
            base.Dispose();
        }
    }
}
