using AnarchyEngine.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

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

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RegisterComponentAsAttribute : Attribute {

        private static readonly Dictionary<Type, MethodInfo> CachedGet, CachedSet, CachedHas;
        private static readonly MethodInfo GetFn, SetFn, HasFn;
        private static readonly object[] _Empty = new object[0];

        public Type Type { get; }

        static RegisterComponentAsAttribute() {
            CachedGet = new Dictionary<Type, MethodInfo>();
            CachedSet = new Dictionary<Type, MethodInfo>();
            CachedHas = new Dictionary<Type, MethodInfo>();

            GetFn = GetMethod("Get");
            SetFn = GetMethod("Set");
            HasFn = GetMethod("Has");
        }

        public RegisterComponentAsAttribute(Type type) {
            Type = type;
        }

        internal bool CallHas(DefaultEcs.Entity handle) {
            if (!CachedHas.TryGetValue(Type, out MethodInfo fn)) {
                fn = HasFn.MakeGenericMethod(Type);
                CachedHas.Add(Type, fn);
            }
            return (bool)fn.Invoke(handle, _Empty);
        }

        internal T CallGet<T>(DefaultEcs.Entity handle) {
            if (!CachedGet.TryGetValue(Type, out MethodInfo fn)) {
                fn = GetFn.MakeGenericMethod(Type);
                CachedGet.Add(Type, fn);
            }
            return (T)fn.Invoke(handle, _Empty);
        }

        internal T CallSet<T>(DefaultEcs.Entity handle, T component) {
            if (!CachedSet.TryGetValue(Type, out MethodInfo fn)) {
                fn = SetFn.MakeGenericMethod(Type);
                CachedSet.Add(Type, fn);
            }
            return (T)fn.Invoke(handle, ArrayHelper.Wrap<object>(component));
        }

        private static MethodInfo GetMethod(string name) {
            var methods = typeof(DefaultEcs.Entity).GetMethods();
            foreach (var m in methods) {
                if (m.IsGenericMethod && m.Name == name) {
                    return m;
                }
            }
            throw new Exception("Could not find method");
        }
    }
}
