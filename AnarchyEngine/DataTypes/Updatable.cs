using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.DataTypes {
    public interface IUpdatable {
        void Start();
        void Render();
        void Update();
    }
    
    public class Updatable : IUpdatable {

        public Action OnRender, OnStart, OnUpdate;

        public void Render() => OnRender?.Invoke();

        public void Start() => OnStart?.Invoke();

        public void Update() => OnUpdate?.Invoke();
    }
}
