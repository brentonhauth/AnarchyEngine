using System;
using System.Collections.Generic;

namespace AnarchyEngine.Core {
    public abstract class Application : IDisposable {
        public static Application Instance { get; private set; }

        public bool Running { get; private set; }

        private Window Window;
        private List<Scene> Scenes;
        private GameSettings Settings;

        public Application(GameSettings settings) {
            Settings = settings;
            Scenes = new List<Scene>();
        }

        ~Application() => Dispose();

        public virtual void Run() {
            using (Window = new Window(Settings.Name, Settings.Width, Settings.Height)) {
                Window.Run(Settings.FPS);
            }
        }

        public virtual void OnInit() {

        }

        public virtual void OnStart() { }

        public virtual void OnUpdate() { }

        public virtual void Exit() {
            Window?.Exit();
        }

        public void Dispose() {
        }
    }

    public class GameSettings {
        public string Name { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public float FPS { get; set; } = 60f;
    }
}
