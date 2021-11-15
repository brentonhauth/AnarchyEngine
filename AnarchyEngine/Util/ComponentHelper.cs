using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AnarchyEngine.ECS.Components;

namespace AnarchyEngine.Util {
    internal static class ComponentHelper {
        private static readonly Dictionary<Type, ComponentMetaAttribute> MetaCache
            = new Dictionary<Type, ComponentMetaAttribute>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assert<T>() where T : Component {
#if DEBUG
            if (typeof(T) == typeof(Component)) {
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

            meta = TypeHelper.GetAttribute<ComponentMetaAttribute, T>();
            return MetaCache[type] = meta;
        }
    }
}
