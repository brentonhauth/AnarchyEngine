﻿using AnarchyEngine.Core;
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
        private static List<RenderContext> Contexts;

        public static event Action ScheduleForInit;

        public static void Init() {
            CurrentContext = new RenderContext();
            Contexts = new List<RenderContext>();
            ScheduleForInit?.Invoke();
        }

        public static void Start() => Start(Camera.Main);

        public static void Start(Camera camera) {
            Camera = camera;
            ViewProjection = camera.ViewProjection;
        }

        public static void Push(Material material, VertexArray va, Matrix4 transform) {
            CurrentContext.Material = material;
            CurrentContext.VertexArray = va;
            CurrentContext.Transform = transform;
        }

        /*public static void Push(string name, int val) => Push(name, RenderContextExtra.Int(val));
        public static void Push(string name, float val) => Push(name, RenderContextExtra.Float(val));
        public static void Push(string name, Vector3 vec) => Push(name, RenderContextExtra.Vec3(vec));
        public static void Push(string name, Matrix4 mat) => Push(name, RenderContextExtra.Mat4(mat));
        private static void Push(string name, RenderContextExtra extra) {
            if (CurrentContext.Extras == null) {
                CurrentContext.Extras = new Dictionary<string, RenderContextExtra>();
            }
            CurrentContext.Extras.Add(name, extra);
        }*/

        public static void Submit() {
            Contexts.Add(CurrentContext);
            CurrentContext = new RenderContext();
        }

        public static void Finish() {
            foreach (var c in Contexts) {
                Shader shader = c.Material.Shader;

                shader.Use();
                c.VertexArray.Use();

                //<temp>
                shader.SetVector3("viewPos", Camera.Main.Front);
                shader.SetVector3("lightPos", Camera.Main.Position);
                //</temp>

                shader.SetMatrix4(Shader.ModelName, c.Transform);
                shader.SetMatrix4(Shader.ViewProjectionName, ref ViewProjection);

                c.Material.ApplyShader();
                Draw(c.VertexArray);
            }
            Contexts.Clear();
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
