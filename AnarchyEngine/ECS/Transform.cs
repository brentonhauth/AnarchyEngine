using System;
using AnarchyEngine.DataTypes;
using Quaternion = OpenTK.Quaternion;
using Matrix4 = OpenTK.Matrix4;

namespace AnarchyEngine.ECS {
    public sealed class Transform {
        private Vector3 m_Position, m_Scale;
        private Quaternion m_Rotation;

        public Vector3 Position {
            get => m_Position;
            set {
                m_Position = value;
                var e = Entity;
                var v = e.Events;
                v.RaiseUpdatePosition(ref m_Position);
            }
        }

        public Vector3 Scale {
            get => m_Scale;
            set {
                m_Scale = value;
                Entity.Events.RaiseUpdateScale(ref m_Scale);
            }
        }

        public Quaternion Rotation {
            get => m_Rotation;
            set {
                m_Rotation = value;
                Entity.Events.RaiseUpdateRotation(ref m_Rotation);
            }
        }

        public Entity Entity { get; private set; }

        public Matrix4 Model => Matrix4.CreateScale(Scale)
            * Matrix4.CreateFromQuaternion(Rotation)
            * Matrix4.CreateTranslation(Position);
        
        public Transform(Entity entity) {
            Entity = entity;
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = new Quaternion(Vector3.Zero);
        }

        public Transform() {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = new Quaternion(Vector3.Zero);
        }

        internal void SetPositionSilently(Vector3 position) => m_Position = position;
        internal void SetRotationSilently(Quaternion rotation) => m_Rotation = rotation;
        internal void SetScaleSilently(Vector3 scale) => m_Scale = scale;

        public static explicit operator DataTypes.Matrix4(Transform t) => t.Model;
        public static explicit operator Matrix4(Transform t) => t.Model;
        public static explicit operator Transform(Matrix4 mat4) {
            return new Transform {
                Position = mat4.ExtractTranslation(),
                Rotation = mat4.ExtractRotation(),
                Scale = mat4.ExtractScale(),
            };
        }
    }
}
