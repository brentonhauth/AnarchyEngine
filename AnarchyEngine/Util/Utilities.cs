using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Util {
    public static class Utilities {

        internal static string CreateGuid() {
            return Guid.NewGuid().ToString("N");
        }
        public static void Swap<T>(ref T var0, ref T var1) {
            T temp = var0;
            var0 = var1;
            var1 = temp;
        }

        public static T Cast<T>(object @object, T @default) {
            try {
                T value = (T)@object;
                return value;
            } catch (Exception) { }
            return @default;
        }
    }
}
