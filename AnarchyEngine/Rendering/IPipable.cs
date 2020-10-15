using System;

namespace AnarchyEngine.Rendering {
    public interface IPipable : IDisposable {
        int Handle { get; }
        void Init();
        void Use();
    }

    public interface ILinkable<T> : IPipable where T : IPipable {
        void Link(T pipe);
    }
}
