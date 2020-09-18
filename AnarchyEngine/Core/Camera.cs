using AnarchyEngine.Util;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = AnarchyEngine.DataTypes.Vector3;

namespace AnarchyEngine.Core {
    public class Camera {
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private float m_pitch,
            m_yaw = -MathHelper.PiOver2,
            m_fov = MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio) {
            Position = position;
            AspectRatio = aspectRatio;
        }

        public Vector3 Position { get; set; }
        public float AspectRatio { get; set; }

        public Vector3 Right { get; private set; } = Vector3.UnitX;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Vector3 Front { get; private set; } = -Vector3.UnitZ;

        public Matrix4 View =>
            Matrix4.LookAt(Position, Position + Front, Up);
        public Matrix4 Projection =>
            Matrix4.CreatePerspectiveFieldOfView(m_fov, AspectRatio, .01f, 100f);

        public Matrix4 ViewProjection => View * Projection;

        public float Pitch {
            get => MathHelper.RadiansToDegrees(m_pitch);
            set {
                var angle = Maths.Clamp(value, -89f, 89f);
                m_pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        
        public float Yaw {
            get => MathHelper.RadiansToDegrees(m_yaw);
            set {
                m_yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float Fov {
            get => MathHelper.RadiansToDegrees(m_fov);
            set {
                var angle = Maths.Clamp(value, 1f, 45f);
                m_fov = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public Matrix4 GetViewMatrix() {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }
        
        public Matrix4 GetProjectionMatrix() {
            Matrix4.CreatePerspectiveFieldOfView(m_fov, AspectRatio, .01f, 100f, out Matrix4 m);
            return m;
        }

        private void UpdateVectors() {
            //Game.SkyBox = ColorHelper.RandColor;
            // First the front matrix is calculated using some basic trigonometry
            //m_front.X = Maths.Cos(m_pitch) * Maths.Cos(m_yaw);
            //m_front.Y = Maths.Sin(m_pitch);
            //m_front.Z = Maths.Cos(m_pitch) * Maths.Sin(m_yaw);

            Front = Maths.CalculateFront(m_pitch, m_yaw);
            Front = Front.Normalized;
            Right = Vector3.Cross(Front, Vector3.UnitY).Normalized;
            Up = Vector3.Cross(Right, Front).Normalized;
        }

        public void Start() {

        }

        public void Awake() {

        }

        public void Render() {

        }


        public void Update() {
            float cameraSpeed = 2f;
            const float sensitivity = 0.3f;

            if (Input.IsKeyDown(Key.LeftCtrl)) {
                cameraSpeed = 4f;
            }

            if (Input.IsKeyDown(Key.W))
                Position += Front * cameraSpeed * (float)Time.DeltaTime; // Forward 
            if (Input.IsKeyDown(Key.S))
                Position -= Front * cameraSpeed * (float)Time.DeltaTime; // Backwards
            if (Input.IsKeyDown(Key.A))
                Position -= Right * cameraSpeed * (float)Time.DeltaTime; // Left
            if (Input.IsKeyDown(Key.D))
                Position += Right * cameraSpeed * (float)Time.DeltaTime; // Right
            if (Input.IsKeyDown(Key.Space))
                Position += Up * cameraSpeed * (float)Time.DeltaTime; // Up 
            if (Input.IsKeyDown(Key.LeftShift))
                Position -= Up * cameraSpeed * (float)Time.DeltaTime; // Down

            /*if (Input.IsKeyPressed(Key.R)) {
                ApplyRotation = !ApplyRotation;
            }

            if (Input.IsKeyPressed(Key.F11)) {
                WindowState = WindowState == 0 ? WindowState.Fullscreen : 0;
            }*/

            /*if (Input.IsKeyPressed(Key.Q)) {
                foreach (var cube in Cubes) {
                    var norm = (cube.Transform.Position - camera.Position).Normalized();
                    cube.Transform.Position += norm * 10f;
                }
            }*/


            var mouse = Mouse.GetState();

            if (_firstMove) {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            } else {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                var newPos = new Vector2(mouse.X, mouse.Y);
                if (newPos != _lastPos) {
                    Yaw += deltaX * sensitivity;
                    Pitch -= deltaY * sensitivity;
                    _lastPos = newPos;
                    //Game.SkyBox = Color4.FromXyz(new Vector4(
                    //    new Vector3(newPos.X, newPos.Y, newPos.X + newPos.Y).Normalized(), 1));
                }

            }

            // lightPos = Position;
        }
    }
}
