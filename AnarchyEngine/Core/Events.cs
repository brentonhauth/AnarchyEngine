using System;

namespace AnarchyEngine.Core {
    public static class Events {
        public static event Action OnStart;
        public static event Action OnPreUpdate;
        public static event Action OnUpdate;
        public static event Action OnPostUpdate;
        public static event Action<int, int> OnWindowResize;
        public static event Action<float, float> OnMouseMove;
        public static event Action<Key> OnKeyUp;
        public static event Action<Key> OnKeyDown;

        internal static void RaiseStart() => OnStart?.Invoke();
        internal static void RaisePreUpdate() => OnPreUpdate?.Invoke();
        internal static void RaiseUpdate() => OnUpdate?.Invoke();
        internal static void RaisePostUpdate() => OnPostUpdate?.Invoke();
        internal static void RaseWindowResize(int width, int height) => OnWindowResize?.Invoke(width, height);
        internal static void RaiseMouseMove(float x, float y) => OnMouseMove?.Invoke(x, y);
        internal static void RaiseKeyUp(Key key) => OnKeyUp?.Invoke(key);
        internal static void RaiseKeyDown(Key key) => OnKeyDown?.Invoke(key);

    }
}
