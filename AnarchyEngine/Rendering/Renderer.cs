using AnarchyEngine.Core;
using AnarchyEngine.ECS;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using OpenTK;
// using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering {
    internal static class Renderer {
        public static Camera Camera { get; private set; }
        public static Entity CurrentEntity { get; private set; }
        private static RenderContext CurrentContext, Context;
        private static Queue<RenderContext> Contexts;

        public static event Action ScheduleForInit;

        public static void Init() {
            Context = new RenderContext();
            CurrentContext = new RenderContext();
            Contexts = new Queue<RenderContext>();
            ScheduleForInit?.Invoke();
        }

        // Rewrite
        public static void Push(Entity entity) {
            CurrentEntity = entity;
        }

        public static void Start() => Start(Camera.Main);

        public static void Start(Camera camera) {
            Camera = camera;
            Context.ViewProjection = camera.ViewProjection;
        }

        public static void Push(Texture texture) {
            CurrentContext.Texture = texture;
        }

        public static void Push(Shader shader, VertexArray va) {
            Push(shader, va, Material.Default, (Matrix4)CurrentEntity.Transform);
        }

        public static void Push(Shader shader, VertexArray va, Material material, Transform transform) {
            Push(shader, va, material, (Matrix4)transform);
        }

        public static void Push(Shader shader, VertexArray va, Material material, Matrix4 transform) {
            CurrentContext.Shader = shader;
            CurrentContext.VertexArray = va;
            CurrentContext.Transform = transform;
            CurrentContext.Material = material;
            CurrentContext.ViewProjection = Context.ViewProjection;
        }

        public static void Push(string name, int val) => Push(name, RenderContextExtra.Int(val));
        public static void Push(string name, float val) => Push(name, RenderContextExtra.Float(val));
        public static void Push(string name, Vector3 vec) => Push(name, RenderContextExtra.Vec3(vec));
        public static void Push(string name, Matrix4 mat) => Push(name, RenderContextExtra.Mat4(mat));
        private static void Push(string name, RenderContextExtra extra) {
            if (CurrentContext.Extras == null) {
                CurrentContext.Extras = new Dictionary<string, RenderContextExtra>();
            }
            CurrentContext.Extras.Add(name, extra);
        }

        public static void Submit() {
            Contexts.Enqueue(CurrentContext);
            CurrentContext = new RenderContext();
        }

        public static void Finish() {
            while (Contexts.Count > 0) {
                RenderContext ctx = Contexts.Dequeue();
                Shader shader = ctx.Material.Shader;
                VertexArray va = ctx.VertexArray;

                ctx.Texture?.Use();//temp
                va.Use();
                shader.Use();
                //<temp>
                shader.SetVector3("viewPos", Camera.Main.Front);
                shader.SetVector3("lightPos", Camera.Main.Position);
                //</temp>
                shader.SetMatrix4(Shader.ModelName, ctx.Transform);
                shader.SetMatrix4(Shader.ViewProjectionName, ctx.ViewProjection);
                ctx.Material?.ApplyToShader(shader);
                if (ctx.Extras != null) {
                    foreach (var extra in ctx.Extras) {
                        var value = extra.Value;
                        shader.Set(value.Type, extra.Key, value.Data);
                    }
                }
                Draw(va);
            }
            CurrentContext = new RenderContext();
        }

        public static void Draw(VertexArray va) {
            GL.DrawArrays(PrimitiveType.Triangles, 0, va.Count);
        }

        public static void PreCleanupBind() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
