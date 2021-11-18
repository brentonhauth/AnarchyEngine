using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Core {
    public interface IApplication : IDisposable {
        GameSettings Settings { get; }
        void Setup();
        void Init();
        void Start();
        void PreUpdate(in float deltaTime);
        void Update(in float deltaTime);
        void PostUpdate(in float deltaTime);
        void Render();
        void Exit();
    }

    public interface IRunner : IDisposable {
        void Run(IApplication app);
    }

    public interface IWindow : IDisposable {
        void SwapBuffers();
        void Exit();
    }
}
