using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {
    public interface ISingleComponent { }

    public class Component : IDisposable {
        public Entity Entity { get; internal set; }

        public Component() { }

        public virtual void Init() { }

        public virtual void Start() { }

        public virtual void Render() { }

        public virtual void Update() { }

        public virtual void AppendTo(Entity entity) {
            Entity = entity;
        }

        public Component(Entity entity) {
            Entity = entity;
        }

        public virtual void Dispose() { }

        public static implicit operator bool(Component comp) => comp != null;
    }
}
