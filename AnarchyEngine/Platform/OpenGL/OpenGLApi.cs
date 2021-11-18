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
using OpenTK.Graphics.OpenGL4;
using Matrix4 = AnarchyEngine.DataTypes.Matrix4;

namespace AnarchyEngine.Platform.OpenGL {
    internal class OpenGLApi : RendererApi {
        public OpenGLApi() : base() { }
        
        public override void Init() {
        }

        public override void Dispose() {
        }

        public override void Draw(VertexArray va, Primitive type = Primitive.Triangles) {
            GL.DrawArrays((PrimitiveType)type, 0, va.Count);
        }

        public override void DrawIndexed(VertexArray va, Primitive type = Primitive.Triangles) {
            throw new NotImplementedException();
        }

        public override void Submit(in Camera camera, in RenderContext ctx, ref Matrix4 viewProjection) {
            var shader = ctx.Material.Shader;
            var va = ctx.VertexArray;

            va.Use();
            shader.Use();

            shader.SetVector3("viewPos", camera.Position);
            shader.SetVector3("lightPos", camera.Position); // temp

            shader.SetMatrix4(Shader.ViewProjectionName, viewProjection);
            shader.SetMatrix4(Shader.ModelName, ctx.Transform);

            ctx.Material.ApplyShader();
            Draw(va);
        }

        public override void PreCleanUp() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
