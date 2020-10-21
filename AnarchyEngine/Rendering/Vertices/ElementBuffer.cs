using OpenTK.Graphics.OpenGL4;

namespace AnarchyEngine.Rendering.Vertices {
    internal class ElementBuffer : IPipable {

        private uint[] Indices;

        public int Handle { get; private set; }
        public int Count => Indices.Length;

        public ElementBuffer() { }

        public ElementBuffer(uint[] indices) {
            Indices = indices;
        }

        public void Init() {
            Handle = GL.GenBuffer();

            Use();
            GL.BufferData(
                target: BufferTarget.ElementArrayBuffer,
                size: Indices.Length * sizeof(uint),
                data: Indices,
                usage: BufferUsageHint.StaticDraw);
        }

        public void Use() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);

        public void Dispose() {
            //GL.DeleteBuffer(Handle);
        }
    }
}
