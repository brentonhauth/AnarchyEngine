using System;
using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Key = AnarchyEngine.Core.Key;
using Vector3 = AnarchyEngine.DataTypes.Vector3;

namespace AnarchyEngine.Platform.OpenGL {
    internal class GLWindow : GameWindow, IWindow, IRunner {

        public IApplication Application { get; private set; }
        
        internal GLWindow(string title, int width, int height) : base(width, height, GraphicsMode.Default, title) {
            X = Y = 30;
            //WindowState = WindowState.Fullscreen;
            Camera.Main = new Camera(Vector3.One * .25f, (float)width / height);
        }

        public void Run(IApplication app) {
            Application = app;
            var settings = app.Settings;
            base.Run(settings.FPS);
        }

        protected override void OnLoad(EventArgs e) {
            Application.Init();
            Application.Start();

            CursorVisible = false;
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            Application.Render();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            float deltaTime = (float)e.Time;
            Application.PreUpdate(in deltaTime);
            Application.Update(in deltaTime);
            Application.PostUpdate(in deltaTime);

            /*<temp>*/
            if (Input.IsKeyPressed(Key.Escape)) Application.Exit();

            base.OnUpdateFrame(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e) {
            base.OnKeyDown(e);
        }

        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            Camera.Main.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (Focused) {
                Mouse.SetPosition(X + Width * .5f, Y + Height * .5f);
            }
            base.OnMouseMove(e);
        }

        protected override void OnUnload(EventArgs e) {
            Renderer.PreCleanUp();
            World.Dispose();
            base.OnUnload(e);
        }

    }
}
