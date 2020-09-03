using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Util {
    public static class ArrayHelper {

        public static T First<T>(this T[] list) => list[0];

        public static T Last<T>(this T[] list) => list[list.Length - 1];

        public static T[] Weave<T>(T[] first, T[] second, int f, int s) {
            var weaved = new List<T>();
            Queue<T> q1 = new Queue<T>(first),
                q2 = new Queue<T>(second);

            while (q1.Count > 0 || q2.Count > 0) {
                weaved.AddRange(q1.Pop(f));
                weaved.AddRange(q2.Pop(s));
            }

            return weaved.ToArray();
        }

        public static T Pop<T>(this Queue<T> q) => q.Dequeue();

        public static T[] Pop<T>(this Queue<T> queue, int amount) {
            amount = OpenTK.MathHelper.Clamp(amount, 0, queue.Count);

            var popped = new T[amount];

            for (var i = 0; i < amount; i++) {
                popped[i] = queue.Dequeue();
            }

            return popped;
        }

        public static T[] Concat<T>(this T[] a1, T[] a2) {
            var a3 = new T[a1.Length + a2.Length];
            a1.CopyTo(a3, 0);
            a2.CopyTo(a3, a2.Length);
            return a3;
        }

        public static T[] CloneList<T>(IEnumerable<T> list) {
            var cloned = new List<T>();
            foreach (var item in list) {
                cloned.Add(item);
            }

            return cloned.ToArray();
        }

        public static T[] Wrap<T>(T item) => new[] { item };
    }
}
