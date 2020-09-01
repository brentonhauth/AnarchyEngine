using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.DataTypes {
    public class Pool<T> : IDisposable {

        private Stack<T> Stack = new Stack<T>();

        public T this[int index] {
            get {
                lock (Stack) {
                    return Stack.ElementAt(index);
                }
            }
        }

        public int Count => Stack.Count;

        public T Retrieve() {
            lock (Stack) {
                return Count == 0
                    ? Activator.CreateInstance<T>()
                    : Stack.Pop();
            }
        }

        public void Push(T item) {
            lock (Stack) {
                Stack.Push(item);
            }
        }

        public void Dispose() {
            lock (Stack) {
                Stack.Clear();
            }
        }
    }
}
