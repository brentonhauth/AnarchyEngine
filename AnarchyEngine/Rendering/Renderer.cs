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
        private static RenderContext CurrentContext;
        private static Matrix4 ViewProjection;
        private static Queue<RenderContext> Contexts;

        public static event Action ScheduleForInit;

        public static void Init() {
            CurrentContext = new RenderContext();
            Contexts = new Queue<RenderContext>();
            ScheduleForInit?.Invoke();
        }

        public static void Start() => Start(Camera.Main);

        public static void Start(Camera camera) {
            Camera = camera;
            ViewProjection = camera.ViewProjection;
        }
        

        public static void Push(Shader shader, VertexArray va, Material material, Transform transform) {
            Push(shader, va, material, (Matrix4)transform);
        }

        public static void Push(Shader shader, VertexArray va, Material material, Matrix4 transform) {
            CurrentContext.Shader = shader;
            CurrentContext.VertexArray = va;
            CurrentContext.Transform = transform;
            CurrentContext.Material = material;
            CurrentContext.ViewProjection = ViewProjection;
        }

        public static void Push(string name, Material material, VertexArray va, Matrix4 transform) {
            CurrentContext.Name = name;
            CurrentContext.Material = material;
            CurrentContext.VertexArray = va;
            CurrentContext.Transform = transform;
            CurrentContext.ViewProjection = ViewProjection;
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

        public static void Finish2() {
            while (true) {
                if (Contexts.Count == 0) break;
                var ctx = Contexts.Dequeue();
                Shader shader = ctx.Material.Shader;

                shader.Use();
                ctx.VertexArray.Use();

                //<temp>
                shader.SetVector3("viewPos", Camera.Main.Front);
                shader.SetVector3("lightPos", Camera.Main.Position);
                //</temp>

                shader.SetMatrix4(Shader.ModelName, ctx.Transform);
                shader.SetMatrix4(Shader.ViewProjectionName, ctx.ViewProjection);

                ctx.Material.ApplyShader();

                if (Time.Ticks % 120 == 0) {
                    int vbo = ctx.VertexArray.VertexBuffer.Handle,
                        vao = ctx.VertexArray.Handle;
                    Console.WriteLine($"{ctx.Name} -> VAO: {vao}, VBO: {vbo}");
                }
                Draw(ctx.VertexArray);
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
