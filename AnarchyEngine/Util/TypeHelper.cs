using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Util {
    internal static class TypeHelper {
        public static Attr[] GetAttributes<Attr, From>() where Attr : Attribute {
            var attrs = typeof(From).GetCustomAttributes(typeof(Attr), false);
            if (attrs.Length == 0) {
                return Array.Empty<Attr>();
            }
            return attrs as Attr[];
        }

        public static Attr GetAttribute<Attr, From>() where Attr : Attribute {
            var attrs = GetAttributes<Attr, From>();
            return attrs.Length > 0 ? attrs[0] : null;
        }
    }
}
