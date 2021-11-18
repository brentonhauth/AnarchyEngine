using AnarchyEngine.Core;
using AnarchyEngine.ECS;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using OpenTK;
// using OpenTK.Graphics.ES20;
using Matrix4 = AnarchyEngine.DataTypes.Matrix4;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering {
    internal static class Renderer {

        public static RendererApi Api { get; private set; }

        //public static Renderer I { get; private set; }

        private static Camera Camera;
        private static RenderContext CurrentContext;
        private static Matrix4 ViewProjection;
        private static List<RenderContext> Contexts;

        public static event Action ScheduleForInit;

        public static void Init() {
            Api = RendererApi.Create();
            Api.Init();
            CurrentContext = new RenderContext();
            Contexts = new List<RenderContext>();
            ScheduleForInit?.Invoke();
        }

        public static void PreRender() {
            Api.PreRender();
        }

        public static void Start() => Start(Camera.Main);

        public static void Start(Camera camera) {
            Camera = camera;
            ViewProjection = camera.ViewProjection;
        }

        public static void Push(Material material, VertexArray va, in Matrix4 transform) {
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
                Api.Submit(in Camera, in c, ref ViewProjection);
            }
            Contexts.Clear();
        }
        
        public static void PreCleanUp() {
            Api.PreCleanUp();
        }
    }

    internal interface IRenderContext { }
}
