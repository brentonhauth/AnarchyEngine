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

        private float m_pitch = 0,
            m_aspectRatio = 0,
            m_yaw = -Maths.HalfPi,
            m_fov = Maths.HalfPi;

        private Matrix4 m_perspective = Matrix4.Identity;

        public Camera(Vector3 position, float aspectRatio) {
            Position = position;
            AspectRatio = aspectRatio;
        }

        public Vector3 Position { get; set; }
        public float AspectRatio {
            get => m_aspectRatio;
            set {
                m_aspectRatio = value;
                UpdatePerspective();
            }
        }

        public Vector3 Right { get; private set; } = Vector3.UnitX;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Vector3 Front { get; private set; } = -Vector3.UnitZ;

        public Matrix4 View =>
            DataTypes.Matrix4.LookAt(Position, Position + Front, Up);
        public Matrix4 Projection => m_perspective;

        public Matrix4 ViewProjection => View * Projection;

        public float Pitch {
            get => Maths.Rad2Deg(m_pitch);
            set {
                var angle = Maths.Clamp(value, -89f, 89f);
                m_pitch = Maths.Deg2Rad(angle);
                UpdateVectors();
            }
        }
        
        public float Yaw {
            get => Maths.Rad2Deg(m_yaw);
            set {
                m_yaw = Maths.Deg2Rad(value);
                UpdateVectors();
            }
        }

        public float Fov {
            get => Maths.Rad2Deg(m_fov);
            set {
                var angle = Maths.Clamp(value, 1f, 45f);
                m_fov = Maths.Deg2Rad(angle);
                UpdateVectors();
                UpdatePerspective();
            }
        }

        private void UpdateVectors() {
            //m_front.X = Maths.Cos(m_pitch) * Maths.Cos(m_yaw);
            //m_front.Y = Maths.Sin(m_pitch);
            //m_front.Z = Maths.Cos(m_pitch) * Maths.Sin(m_yaw);

            Front = Maths.CalculateFront(m_pitch, m_yaw);
            Front = Front.Normalized;
            Right = Vector3.Cross(Front, Vector3.UnitY).Normalized;
            Up = Vector3.Cross(Right, Front).Normalized;
        }

        private void UpdatePerspective() {
            m_perspective = Matrix4.CreatePerspectiveFieldOfView(
                fovy: m_fov,
                aspect: AspectRatio,
                zNear: .01f,
                zFar: 100f);
        }

        public void Start() {

        }

        public void Awake() {

        }

        public void Render() {

        }


        public void Update() {
        }
    }
}
