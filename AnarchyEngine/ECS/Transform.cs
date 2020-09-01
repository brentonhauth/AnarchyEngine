using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS {
    public sealed class Transform {

        private Vector3 m_Position;
        public Vector3 Position {
            get => m_Position;
            set {
                m_Position = value;
                OnUpdate?.Invoke(UpdateFactor.Position);
            }
        }

        private Vector3 m_Scale;
        public Vector3 Scale {
            get => m_Scale;
            set {
                m_Scale = value;
                OnUpdate?.Invoke(UpdateFactor.Scale);
            }
        }

        private Quaternion m_Rotation;
        public Quaternion Rotation {
            get => m_Rotation;
            set {
                m_Rotation = value;
                OnUpdate?.Invoke(UpdateFactor.Rotation);
            }
        }

        public Entity Entity { get; private set; }

        public Matrix4 Model => Matrix4.CreateScale(Scale)
            * Matrix4.CreateFromQuaternion(Rotation)
            * Matrix4.CreateTranslation(Position);

        internal event Action<UpdateFactor> OnUpdate;

        public Transform(Entity entity) : this() {
            Entity = entity;
        }

        public Transform() {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = new Quaternion(Vector3.Zero);
        }

        internal void SetPositionSilently(Vector3 position) => m_Position = position;
        internal void SetRotationSilently(Quaternion rotation) => m_Rotation = rotation;
        internal void SetScaleSilently(Vector3 scale) => m_Scale = scale;

        public static explicit operator Matrix4(Transform t) => t.Model;
        public static explicit operator Transform(Matrix4 mat4) {
            return new Transform {
                Position = mat4.ExtractTranslation(),
                Rotation = mat4.ExtractRotation(),
                Scale = mat4.ExtractScale(),
            };
        }

        internal enum UpdateFactor {
            None = 0,
            Position = 1,
            Rotation = 2,
            Scale = 4,
        }
    }
}
