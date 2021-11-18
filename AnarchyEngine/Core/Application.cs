using AnarchyEngine.Platform.OpenGL;
using AnarchyEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.RuntimeInformation;

namespace AnarchyEngine.Core {
    public abstract class Application : IDisposable {
        public static Application Instance { get; private set; }

        public bool Running { get; private set; }
        public GameSettings Settings { get; }
        internal API API { get; private set; }
        internal OS OS { get; private set; }
        private GLWindow Window;
        private List<Scene> Scenes;


        public Application(GameSettings settings) {
            DeterminePlatform();
            Instance = this;
            Settings = settings;
            Scenes = new List<Scene>();
            
            Events.OnStart += OnStart;
            Events.OnUpdate += OnUpdate;
        }

        private void DeterminePlatform() {
            if (IsOSPlatform(OSPlatform.Windows)) {
                OS = OS.Windows;
                API = API.OpenGL;
            } else if (IsOSPlatform(OSPlatform.Linux)) {
                OS = OS.Linux;
                API = API.OpenGL;
            } else if (IsOSPlatform(OSPlatform.OSX)) {
                OS = OS.Mac;
                API = API.OpenGL;
            } else {
                throw new Exception("Invalid OS");
            }
        }

        ~Application() => Dispose();

        public virtual void Run() {
            using (Window = new GLWindow(Settings.Name, Settings.Width, Settings.Height)) {
                Window.Run(Settings.FPS);
            }
        }

        public abstract void Setup();

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

        
        public GameSettings() { }

        public GameSettings(string name) {
            Name = name;
        }

        public GameSettings(string name, int width, int height) {
            Name = name;
            Width = width;
            Height = height;
        }

        public GameSettings(string name, int width, int height, float fps) {
            Name = name;
            Width = width;
            Height = height;
            FPS = fps;
        }
    }

    internal enum OS {
        None,
        Windows,
        Linux,
        Mac,
    }

    internal interface IRunnable {
        void Init();
        void Start();
        void PreUpdate();
        void Update();
        void PostUpdate();
        void End();
        void Shutdown();
    }
}
