using OpenTK.Graphics.ES20;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AnarchyEngine.Rendering.Vertices {
    internal class VertexBuffer : IPipable {
        public int Handle { get; private set; }
        public int Count => Data?.Length ?? 0;

        public float[] Data { get; private set; }

        public VertexBuffer() { }

        public VertexBuffer(float[] data) {
            Data = data;
        }

        public void Init() {
            Handle = GL.GenBuffer();

            Use();
            GL.BufferData(
                target: BufferTarget.ArrayBuffer,
                size: Data.Length * sizeof(float),
                data: Data,
                usage: BufferUsageHint.StaticDraw);
        }

        public void SetData(float[] data) {
            Data = data;
        }

        public void Use() => GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);

        public void Dispose() {
            GL.DeleteBuffer(Handle);
        }
    }
}
