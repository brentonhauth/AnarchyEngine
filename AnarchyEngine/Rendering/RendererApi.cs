using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.Platform.OpenGL;
using AnarchyEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace AnarchyEngine.Rendering {
    internal abstract class RendererApi : IDisposable {

        public RendererApi() {}
        public abstract void Init();

        public abstract void Draw(VertexArray va, Primitive type=Primitive.Triangles);

        public abstract void DrawIndexed(VertexArray va, Primitive type=Primitive.Triangles);

        public abstract void Submit(in Camera camera, in RenderContext ctx, ref Matrix4 ViewProjection);

        public abstract void PreCleanUp();

        public abstract void Dispose();

        public abstract void PreRender();

        
        public static RendererApi Create() {
            API api = API.OpenGL;
            switch (api) {
                case API.OpenGL:
                    return new OpenGLApi();
                case API.None:
                default:
                    throw new Exception();
            }
        }
    }

    internal enum Primitive {
        Triangles = 4,
    }

    internal enum API {
        None = 0,
        OpenGL = 1,
        Vulkan = 2,
        DirectX = 3,
    }
}
