using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {

    public enum ComponentId {
        Transform = 1,
        MeshFilter = 2,
        RigidBody = 3,
        Collider = 4,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class ComponentMetaAttribute : Attribute {
        public ComponentId Id { get; }
        public bool AllowMultiple { get; set; }
        public Type RegisterAs { get; set; }

        public ComponentMetaAttribute(ComponentId id) {
            Id = id;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class RequireComponentsAttribute : Attribute {
        public Type[] Types { get; }

        public RequireComponentsAttribute(params Type[] types) {
            // TODO: Check types
            Types = types;
        }
    }
}
