using AnarchyEngine.Physics;
using Jitter.Collision.Shapes;

namespace AnarchyEngine.ECS.Components {
    public abstract class Collider : Component {
        public BBox Bounds { get; protected set; } = new BBox();
        public virtual Shape Shape { get; protected set; }

        public Collider() : base() { }

        public void AddedComponentListener(Component comp) {
            if (!(comp is RigidBody)) return;
        }

        public override void AppendTo(Entity entity) {
            base.AppendTo(entity);
            entity.Events.AddedComponent += AddedComponentListener;
        }
    }

    public class BoxCollider : Collider {
        public BoxCollider(float length, float height, float width) : base() {
            Shape = new BoxShape(length, height, width);
        }

        public override void Start() { }
    }
}
