using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public static class World {
        public static Window Window { get; private set; }

        public static Camera MainCamera { get; internal set; }

        public static Scene CurrentScene { get; private set; }

        private static Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>(0);

        public static ulong Ticks { get; internal set; } = 0;

        private static Color4 m_SkyBox;
        public static Color4 SkyBox {
            get => m_SkyBox;
            set => GL.ClearColor(m_SkyBox = value);
        }

        public static void Run(string title, int width, int height, double fps = 60.0) {
            using (Window = new Window(title, width, height)) {
                Window.Run(fps);
            }
        }

        internal static void Start() {
            if (CurrentScene == null) {
                CurrentScene = Scenes.ElementAt(0).Value;
            }
            MainCamera?.Start();
            CurrentScene?.Start();
        }

        internal static void Render() {
            MainCamera.Render();
            CurrentScene.Render();
        }

        internal static void Update() {
            MainCamera.Update(); // change camera logic
            CurrentScene.Update();
        }

        public static void LoadScene(string sceneName) {
            if (Scenes.TryGetValue(sceneName, out Scene scene)) {
                CurrentScene = scene;
            } else {
                throw new Exception($"No scene with name \"{sceneName}\"");
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
    }
}
