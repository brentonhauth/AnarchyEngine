using System;
using AnarchyEngine.DataTypes;

namespace AnarchyEngine.Core {
    public abstract class Object : IUpdatable, IDisposable {
        public bool Initialized { get; private set; }

        public virtual void Init() {
            Initialized = true;
        }

        public virtual void Start() {
            if (!Initialized) Init();
        }

        public virtual void Render() { }

        public virtual void Update() { }

        public virtual void Dispose() { }

        public static implicit operator bool(Object obj) => obj != null;
    }
}
