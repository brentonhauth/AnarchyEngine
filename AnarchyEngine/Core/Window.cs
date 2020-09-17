using System;
using AnarchyEngine.Physics;
using AnarchyEngine.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace AnarchyEngine.Core {
    public class Window : GameWindow {
        
        internal Window(string title, int width, int height) : base(width, height, GraphicsMode.Default, title) {
            X = Y = 30;
            WindowState = WindowState.Fullscreen;
            World.MainCamera = new Camera(Vector3.One * .25f, (float)width / height);
        }

        protected override void OnLoad(EventArgs e) {
            GL.ClearColor(Color4.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            PhysicsSystem.Init();
            Renderer.Init();

            World.Start();

            CursorVisible = false;
            
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Renderer.Start();
            World.Render();
            Renderer.Finish();

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        private void PreUpdate(FrameEventArgs e) {
            Time.DeltaTime = e.Time;
            Time.TotalTime += (decimal)e.Time;
            World.Ticks++;
            Input.UpdateState();
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            PreUpdate(e);
            // ...
            PhysicsSystem.Update();
            World.Update();

            /*<temp>*/ if (Input.IsKeyPressed(Key.Escape)) Exit();

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e) {
            GL.Viewport(0, 0, Width, Height);
            World.MainCamera.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (Focused) {
                Mouse.SetPosition(X + Width * .5f, Y + Height * .5f);
            }
            base.OnMouseMove(e);
        }

        protected override void OnUnload(EventArgs e) {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            World.CurrentScene.Dispose();

            base.OnUnload(e);
        }
    }
}
