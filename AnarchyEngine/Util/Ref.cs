using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Util {
    public class Ref<T> {

        private T m_Value;

        public T Value {
            get => m_Value;
            set => m_Value = value;
        }

        public ref T Get => ref m_Value;

        public object this[string field] {
            get {
                var info = typeof(T).GetField(field);
                return info.GetValue(m_Value);
            }
            set {
                var info = typeof(T).GetField(field);
                info.SetValue(m_Value, value);
            }
        }

        public Ref(T value) {
            Value = value;
        }

        public static implicit operator T(Ref<T> @ref) => @ref.Value;
        public static implicit operator Ref<T>(T value) => new Ref<T>(value);

        public override bool Equals(object o) {
            if (o is Ref<T> r) {
                return m_Value.Equals(r.m_Value);
            }
            if (o is T t) {
                return m_Value.Equals(t);
            }
            return false;
        }
        public override string ToString() => m_Value.ToString();

        public override int GetHashCode() => m_Value.GetHashCode();
    }
}
