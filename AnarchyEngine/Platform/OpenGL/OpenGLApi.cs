using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnarchyEngine.DataTypes;
using AnarchyEngine.Rendering;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using AnarchyEngine.Core;

namespace AnarchyEngine.Platform.OpenGL {
    internal class OpenGLApi : RendererApi {
        public OpenGLApi() : base() { }
        
        public override void Init() {
            throw new NotImplementedException();
        }

        public override void OnFinish(in RenderContext c) {
            Shader shader = c.Material.Shader;

            shader.Use();
            c.VertexArray.Use();

            //<temp>
            shader.SetVector3("viewPos", Camera.Main.Front);
            shader.SetVector3("lightPos", Camera.Main.Position);
            //</temp>

            shader.SetMatrix4(Shader.ModelName, c.Transform);
            shader.SetMatrix4(Shader.ViewProjectionName, GetViewProjectionRef());

            c.Material.ApplyShader();
            c.VertexArray.Draw();
        }

        public override void Push(Material material, VertexArray va, in Matrix4 transform) {
            throw new NotImplementedException();
        }

        public override void Submit() {
            throw new NotImplementedException();
        }

        public override void Dispose() {
        }
    }
}
