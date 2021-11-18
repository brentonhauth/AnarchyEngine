using AnarchyEngine.Platform.OpenGL;
using AnarchyEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.RuntimeInformation;
using static AnarchyEngine.Platform.Platform;
using AnarchyEngine.ECS;

namespace AnarchyEngine.Core {
    public abstract class Application : IApplication {
        public static Application Instance { get; private set; }

        public ApplicationEvents Events { get; }

        public bool Running { get; private set; }
        public GameSettings Settings { get; }
        internal API API { get; private set; }
        internal OS OS { get; private set; }
        internal IWindow Window { get; private set; }
        internal IRunner Runner { get; private set; }

        private List<Scene> Scenes;


        public Application(GameSettings settings) {
            DeterminePlatform();
            if (Instance != null) {
                throw new Exception();
            }
            Instance = this;
            Settings = settings;
            Events = new ApplicationEvents();
            Scenes = new List<Scene>();
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
            Window = CreateWindow(API, Settings);
            using (Runner = CreateRunner(API, Settings)) {
                Runner.Run(this);
            }
        }

        public abstract void Setup();

        public virtual void Init() {
            CoreECS.Init();
            Physics.Physics.Init();
            Renderer.Init();
        }

        public virtual void Start() {
            Setup();
            World.Start();
            Events.RaiseStart();
        }

        public virtual void PreUpdate(in float deltaTime) {
            Time.Update(in deltaTime);
            Input.UpdateState();
            Events.RaisePreUpdate();
        }

        public virtual void Update(in float deltaTime) {
            Physics.Physics.Update();
            Scheduler.Update();
            World.Update();
            Events.RaiseUpdate();
        }

        public virtual void PostUpdate(in float deltaTime) {
            Events.RaisePostUpdate();
        }

        public virtual void Render() {
            Renderer.Start();
            World.Render();
            Renderer.Finish();

            Window.SwapBuffers();
        }

        public virtual void Exit() {
            Window?.Exit();
        }

        public void Dispose() {
        }

        #region Application Events
        public class ApplicationEvents {
            public event Action OnStart;
            public event Action OnPreUpdate;
            public event Action OnUpdate;
            public event Action OnPostUpdate;
            public event Action<int, int> OnWindowResize;
            public event Action<float, float> OnMouseMove;
            public event Action<Key> OnKeyUp;
            public event Action<Key> OnKeyDown;

            internal void RaiseStart() => OnStart?.Invoke();
            internal void RaisePreUpdate() => OnPreUpdate?.Invoke();
            internal void RaiseUpdate() => OnUpdate?.Invoke();
            internal void RaisePostUpdate() => OnPostUpdate?.Invoke();
            internal void RaseWindowResize(int width, int height) => OnWindowResize?.Invoke(width, height);
            internal void RaiseMouseMove(float x, float y) => OnMouseMove?.Invoke(x, y);
            internal void RaiseKeyUp(Key key) => OnKeyUp?.Invoke(key);
            internal void RaiseKeyDown(Key key) => OnKeyDown?.Invoke(key);
        }
        #endregion
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
}
