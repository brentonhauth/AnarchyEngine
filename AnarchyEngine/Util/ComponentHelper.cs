using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AnarchyEngine.ECS.Components;
using Handle = DefaultEcs.Entity;

namespace AnarchyEngine.Util {
    internal static class ComponentHelper {
        private static readonly Dictionary<Type, ComponentMetaAttribute> MetaCache
            = new Dictionary<Type, ComponentMetaAttribute>();
        private static readonly Dictionary<Type, RegisterComponentAsAttribute> RegisterCache
            = new Dictionary<Type, RegisterComponentAsAttribute>();

        private static readonly Cache<string, Type, MethodInfo> Accessors
            = new Cache<string, Type, MethodInfo>();
        private static readonly object[] _Empty = new object[0];
        private static readonly Type _TypeOfComponent = typeof(Component);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assert<T>() where T : Component {
#if DEBUG
            if (typeof(T) == _TypeOfComponent) {
                throw new Exception();
            }
#endif
        }

        public static bool AllowMultiple<T>(bool @default=true) where T : Component {
            var meta = GetMeta<T>();
            return meta == null ? @default : meta.AllowMultiple;
        }

        public static ComponentMetaAttribute GetMeta<T>() where T : Component {
            var type = typeof(T);
            
            if (MetaCache.TryGetValue(type, out var meta)) {
                return meta;
            }

            meta = TypeHelper.GetAttribute<ComponentMetaAttribute>(type);
            return MetaCache[type] = meta;
        }

        public static RegisterComponentAsAttribute GetRegisterAs<T>() where T : Component {
            var type = typeof(T);
            if (RegisterCache.TryGetValue(type, out var register)) {
                return register;
            }

            register = TypeHelper.GetAttribute<RegisterComponentAsAttribute>(type);
            return RegisterCache[type] = register;
        }

        internal static T CallGet<T>(in Handle handle) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                return handle.Get<T>();
            }
            return (T)CallMethod("Get", in handle, type, _Empty);
        }

        internal static T CallSet<T>(in Handle handle, T component) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                handle.Set(component);
            } else {
                CallMethod("Set", in handle, type, ArrayHelper.Wrap<object>(component));
            }
            return component;
        }

        internal static bool CallHas<T>(in Handle handle) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                return handle.Has<T>();
            }
            return (bool)CallMethod("Has", in handle, type, _Empty);
        }

        internal static bool CallIsEnabled<T>(in Handle handle) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                return handle.IsEnabled<T>();
            }
            return (bool)CallMethod("IsEnabled", in handle, type, _Empty);
        }

        internal static void CallEnable<T>(in Handle handle) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                handle.Enable<T>();
            } else {
                CallMethod("Enable", in handle, type, _Empty);
            }
        }

        internal static void CallDisable<T>(in Handle handle) where T : Component {
            var type = typeof(T);
            if (type.BaseType == _TypeOfComponent) {
                handle.Disable<T>();
            } else {
                CallMethod("Disable", in handle, type, _Empty);
            }
        }

        private static object CallMethod(string name, in Handle handle, Type type, object[] @params) {
            if (Accessors.TryGet(name, type, out var method)) {
                return method.Invoke(handle, @params);
            } else if (!Accessors.TryGet(name, _TypeOfComponent, out method)) {
                method = GetHandleMethod(name);
                Accessors.Add(name, _TypeOfComponent, method);
            }
            method = method.MakeGenericMethod(Crawl(type));
            Accessors.Add(name, type, method);
            return method.Invoke(handle, @params);
        }

        private static Type Crawl(Type type) {
            if (type == _TypeOfComponent) {
                throw new Exception();
            }
            while (type.BaseType != _TypeOfComponent) {
                type = type.BaseType;
            }
            return type; 
        }
        
        private static MethodInfo GetHandleMethod(string name) {
            var type = typeof(Handle);
            var methods = type.GetMethods();
            foreach (var m in methods) {
                if (m.IsGenericMethod && m.Name == name) {
                    return m;
                }
            }
            throw new Exception();
        }
    }

    #region Cache
    internal class Cache<TSet, TKey, TValue> {

        private readonly Dictionary<TSet, Dictionary<TKey, TValue>> _Cache;

        internal TValue this[TSet set, TKey key] {
            get {
                TryGet(set, key, out var val);
                return val;
            }
            set {
                var inner = GetInner(set);
                inner[key] = value;
            }
        }

        internal Cache() {
            _Cache = new Dictionary<TSet, Dictionary<TKey, TValue>>();
        }

        internal bool TryGet(TSet set, TKey key, out TValue value) {
            var inner = GetInner(set);
            if (inner.TryGetValue(key, out value)) {
                return true;
            }
            value = default;
            return false;
        }

        internal void Add(TSet set, TKey key, TValue value) {
            var inner = GetInner(set);
            inner.Add(key, value);
        }

        private Dictionary<TKey, TValue> GetInner(TSet set) {
            if (!_Cache.TryGetValue(set, out var inner)) {
                inner = new Dictionary<TKey, TValue>();
                _Cache.Add(set, inner);
            }
            return inner;
        }
    }
    #endregion
}
