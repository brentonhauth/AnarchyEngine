using System;
using AnarchyEngine.DataTypes;
using AnarchyEngine.Physics;
using Jitter.Collision.Shapes;

namespace AnarchyEngine.ECS.Components {
    public abstract class Collider : Component {
        public BBox Bounds => Shape.BoundingBox;
        public virtual Shape Shape { get; protected set; }
        
        public Collider() : base() { }

        public void AddedComponentListener(Component comp) {
            if (comp is Collider && comp.Id != Id) {
                throw new Exception();
            } else if (!(comp is RigidBody)) return;
        }

        public override void AppendTo(Entity e) {
            e.Events.AddedComponent += AddedComponentListener;
            base.AppendTo(e);
        }

        public override void Dispose() {
            Entity.Events.AddedComponent -= AddedComponentListener;
        }
    }

    [RegisterComponentAs(typeof(Collider))]
    public class BoxCollider : Collider {
        public Vector3 Size {
            get => (Shape as BoxShape)?.Size ?? Vector3.Zero;
            set { if (Shape is BoxShape s) s.Size = value; }
        }

        public BoxCollider() : this(1, 1, 1) { }

        public BoxCollider(float length, float height, float width) : base() {
            Shape = new BoxShape(length, height, width);
        }

        public override void Start() { }
    }

    public class SphereCollider : Collider {
        public float Radius {
            get => (Shape as SphereShape)?.Radius ?? 0;
            set { if (Shape is SphereShape s) s.Radius = value; }
        }

        public SphereCollider(float radius) : base() {
            Shape = new SphereShape(radius);
        }
        
        public SphereCollider() : this(1) { }
    }
}
