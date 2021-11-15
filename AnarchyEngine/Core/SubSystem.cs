namespace AnarchyEngine.Core {
    internal abstract class SubSystem {
        public abstract void Init();
        public abstract void Start();
        public abstract void PreUpdate();
        public abstract void Update();
        public abstract void PostUpdate();
        public abstract void End();
        public abstract void Shutdown();
    }
}
