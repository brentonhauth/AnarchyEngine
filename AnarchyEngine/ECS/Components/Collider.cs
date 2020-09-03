using AnarchyEngine.Physics;
using Jitter.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {
    public abstract class Collider : Component {
        public BBox Bounds { get; protected set; } = new BBox();
        public virtual Shape Shape { get; protected set; }


        public Collider() : base() { }
    }

    public class BoxCollider : Collider {

        public BoxCollider(float length, float height, float width) : base() {
            Shape = new BoxShape(length, height, width);
        }

        public override void Start() { }
    }
}
