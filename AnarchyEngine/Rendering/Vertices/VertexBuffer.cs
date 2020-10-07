using OpenTK.Graphics.ES20;
using System;

namespace AnarchyEngine.Rendering.Vertices {
    public class VertexBuffer : IDisposable {
        public int VBO { get; private set; }

        private float[] Data;

        public VertexBuffer() {
            Data = new float[0];
        }

        public void Dispose() {
            GL.DeleteBuffer(VBO);
        }
    }
}
