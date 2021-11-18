using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public static class World {
        public static GLWindow Window { get; private set; }
        
        private static Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>(0);

        public static ulong Ticks { get; internal set; } = 0;

        private static Color m_SkyBox;
        public static Color SkyBox {
            get => m_SkyBox;
            set => GL.ClearColor(m_SkyBox = value);
        }

        public static void Run(string title, int width, int height, double fps = 60.0) {
            using (Window = new GLWindow(title, width, height)) {
                Window.Run(fps);
            }
        }

        public static void Init() {
            CoreECS.Init();
            Physics.Physics.Init();
        }

        internal static void Start() {
            if (Scene.Current == null) {
                Scene.Current = Scenes.ElementAt(0).Value;
            }
            Camera.Main.Start();
            Scene.Current.Start();
        }

        internal static void Render() {
            //Scene.Current.Render();
            var renderable = CoreECS.GetRenderable();
            foreach (var r in renderable) {
                r.Render();
            }
        }

        internal static void Update() {
            Camera.Main.Update(); // change camera logic
            Scene.Current.Update();
            var phys = CoreECS.GetPhysicsRelated();
            foreach (var p in phys) {
                p.Update();
            }
        }

        internal static void Dispose() {
            Scene.Current.Dispose();
        }

        public static void LoadScene(string sceneName) {
            if (Scenes.TryGetValue(sceneName, out Scene scene)) {
                Scene.Current = scene;
            } else {
                throw new Exception($"No scene named \"{sceneName}\"");
            }
        }

        public static void LoadScene(Scene scene) {
            try {
                AddScene(scene);
            } catch (Exception) { }
            LoadScene(scene.Name);
        }

        public static void AddScene(Scene scene) {
            if (Scenes.TryGetValue(scene.Name, out _)) {
                throw new Exception("Scene with name already exists!");
            }
            Scenes[scene.Name] = scene;
        }

        private static void InitializeECS() {
            
        }
    }
}
