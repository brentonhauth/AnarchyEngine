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

        public static Camera Main { get; internal set; }

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
            const float sensitivity = .3f;

            float cameraSpeed = (Input.IsKeyDown(Key.LeftCtrl) ? 4f : 2f) * Time.DeltaTime;
            

            if (Input.IsKeyDown(Key.W))
                Position += Front * cameraSpeed; // Forward 
            if (Input.IsKeyDown(Key.S))
                Position -= Front * cameraSpeed;
            if (Input.IsKeyDown(Key.A))
                Position -= Right * cameraSpeed;
            if (Input.IsKeyDown(Key.D))
                Position += Right * cameraSpeed;
            if (Input.IsKeyDown(Key.Space))
                Position += Up * cameraSpeed;
            if (Input.IsKeyDown(Key.LeftShift))
                Position -= Up * cameraSpeed;

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
                }

            }
        }
    }
}
