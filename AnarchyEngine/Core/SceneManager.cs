using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public static class SceneManager {
        private static Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        public static Scene Current { get; private set; }


        internal static void Start() {
            if (Current == null) {
                Current = Scenes.ElementAt(0).Value;
            }
            Current.Start();
        }

        public static void LoadScene(string sceneName) {
            if (Scenes.TryGetValue(sceneName, out Scene scene)) {
                Current = scene;
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
    }
}
