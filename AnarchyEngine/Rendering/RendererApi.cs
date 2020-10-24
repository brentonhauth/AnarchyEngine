using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using AnarchyEngine.Platform.OpenGL;
using AnarchyEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering {
    internal abstract class RendererApi : IDisposable {

        protected Camera Camera;
        protected RenderContext CurrentContext;
        protected Matrix4 ViewProjection;
        protected List<RenderContext> Contexts;

        public RendererApi() {
            Contexts = new List<RenderContext>();
            CurrentContext = new RenderContext();
        }
        protected ref Matrix4 GetViewProjectionRef() => ref ViewProjection;

        public abstract void Init();
        public abstract void Push(Material material, VertexArray va, in Matrix4 transform);
        public abstract void Submit();
        public abstract void OnFinish(in RenderContext ctx);
        public abstract void Dispose();
        
        public void Start(Camera camera) {
            Camera = camera;
            ViewProjection = camera.ViewProjection;
        }

        public virtual void Finish() {
            foreach (var c in Contexts) {
                OnFinish(c);
            }
            Contexts.Clear();
        }

        
        
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

    internal enum API {
        None = 0,
        OpenGL = 1,
    }
}
