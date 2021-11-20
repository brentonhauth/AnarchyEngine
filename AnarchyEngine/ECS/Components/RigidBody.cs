using System;
using OpenTK;
using OpenTK.Graphics.ES20;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Mesh;
using Jitter.LinearMath;
using Jitter.Collision.Shapes;
using JRigidBody = Jitter.Dynamics.RigidBody;

namespace AnarchyEngine.ECS.Components {
    [RequireComponents(typeof(Transform))]
    public class RigidBody : Component {
        private static DebugDrawer drawer = new DebugDrawer();
        private static readonly BoxShape EmptyShape = new BoxShape(JVector.Zero);

        public static bool DebugMode = false;

        internal JRigidBody Body { get; set; }

        private bool _ReceivedShapeFromCollider;
        private JVector _LastPosition = JVector.Zero;
        private DataTypes.Matrix3 _LastRotation = DataTypes.Matrix3.Identity;

        public float Mass {
            get => Body.Mass;
            set => Body.Mass = value;
        }
        
        public bool IsStatic {
            get => Body.IsStatic;
            set => Body.IsStatic = value;
        }

        public bool IsBodyActive {
            get => Body.IsActive;
            set => Body.IsActive = value;
        }

        public bool AffectedByGravity {
            get => Body.AffectedByGravity;
            set => Body.AffectedByGravity = value;
        }

        public DataTypes.Vector3 Velocity {
            get => Body.LinearVelocity;
            set => Body.LinearVelocity = value;
        }

        public RigidBody(float mass) : base() {
            Body = new JRigidBody(EmptyShape);
            Mass = mass;
        }

        public RigidBody() : this(100f) { }

        ~RigidBody() => Dispose();

        internal void ApplyTransformToBody(Transform transform) {
            UpdatePositionListener(transform.Position);
            UpdateRotationListener(transform.Rotation);
        }

        public override void Start() {
            if (DebugMode) drawer.Init();

            if (!_ReceivedShapeFromCollider) {
                var collider = Entity.QuickGet<Collider>();
                if (collider) {
                    Body.Shape = collider.Shape;
                    _ReceivedShapeFromCollider = true;

                }
            }

            var transform = Entity.QuickGet<Transform>();
            var rotation = transform.Rotation;
            
            /*Body.Material.Restitution = float.MinValue;
            Body.Material.KineticFriction = .3f;
            Body.Material.StaticFriction = .6f;*/

            DataTypes.Matrix4 orientation = Matrix4.CreateFromQuaternion(rotation);

            Body.Orientation = (DataTypes.Matrix3)orientation;
            Body.Position = transform.Position;
            //Physics.Physics.Add(Body);
        }

        #region Event Listeners
        private void UpdatePositionListener(DataTypes.Vector3 position) {
            Body.Position = position;
        }

        private void UpdateRotationListener(Quaternion rotation) {
            Matrix4.CreateFromQuaternion(ref rotation, out Matrix4 m);
            Body.Orientation = new JMatrix(
                m.Row0.X, m.Row0.Y, m.Row0.Z,
                m.Row1.X, m.Row1.Y, m.Row1.Z,
                m.Row2.X, m.Row2.Y, m.Row2.Z);
        }

        private void AddedComponentListener(Component comp) {
            Console.WriteLine("ADDED COMP");
            if (comp is RigidBody && comp.Id != Id) {
                throw new Exception();
            } else if (comp is BoxCollider collider) {
                Body.Shape = collider.Shape;
                _ReceivedShapeFromCollider = true;
                Console.WriteLine("OWOOWOWOOWOWO");
            }
        }
        #endregion
        
        public override void Render() {
            if (!DebugMode) return;
            DataTypes.Matrix3 orientation = Body.Orientation;
            DataTypes.Vector3 position = Body.Position;
            
            var rotation = new Matrix4(orientation);
            var model = rotation * Matrix4.CreateTranslation(position);
        }

        public override void Update() {
            if (Body.IsStaticOrInactive) return;

            var transform = Entity.QuickGet<Transform>();
            transform.SetPositionSilently(Body.Position);
            
            JMatrix m = Body.Orientation;
            var rotation = new Matrix3(
                m.M11, m.M21, m.M31,
                m.M12, m.M22, m.M32,
                m.M13, m.M23, m.M33);
            Quaternion.FromMatrix(ref rotation, out Quaternion result);
            transform.SetRotationSilently(in result);
        }

        public override void AppendTo(Entity e) {
            e.Events.UpdatePosition += UpdatePositionListener;
            e.Events.UpdateRotation += UpdateRotationListener;
            e.Events.AddedComponent += AddedComponentListener;
            base.AppendTo(e);
        }

        public override void Dispose() {
            Entity.Events.UpdatePosition -= UpdatePositionListener;
            Entity.Events.UpdateRotation -= UpdateRotationListener;
            Entity.Events.AddedComponent -= AddedComponentListener;
            if (Body != null) {
                Physics.Physics.Remove(Body);
            }
        }
    }

    public class EmptyRigidBody : RigidBody {
        public EmptyRigidBody() : base(1f) {
            IsStatic = true;
        }

        //public override void Update() { }
    }

    public class DebugDrawer {
        private bool initialized = false;

        public DebugDrawer() { }

        public void Init() {
            if (initialized) return; else initialized = true;
            Shader.Default.Init();
            Mesh.Cube.Init();
        }

        public void Draw(in Matrix4 model) {
            var shader = Shader.Default;
            var va = Mesh.Cube.VertexArray;
            va.Use();
            shader.Use();
            shader.SetVector3(Shader.ColorName, new Vector3(1, 1, 0));
            shader.SetMatrix4(Shader.ViewProjectionName, Core.Camera.Main.ViewProjection);
            shader.SetMatrix4(Shader.ModelName, model);

            GL.DrawArrays(PrimitiveType.LineLoop, 0, 36);
        }
    }
}
