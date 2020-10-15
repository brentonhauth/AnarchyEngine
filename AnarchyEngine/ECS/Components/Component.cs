using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {
    public interface ISingleComponent { }

    public class Component : Core.Object {
        private static uint IdCount = 0;

        public readonly uint Id;

        public Entity Entity { get; internal set; }

        public Component() { Id = ++IdCount; }

        public virtual void AppendTo(Entity entity) {
            Entity = entity;
        }

        public Component(Entity entity) {
            Entity = entity;
        }
    }
}
