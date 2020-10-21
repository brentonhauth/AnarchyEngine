using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using System;
using System.Collections.Generic;

namespace AnarchyEngine.Core {
    public class Scene : IDisposable {
        public static Scene Current { get; internal set; }

        public readonly string Name;

        private readonly List<Entity> EntitiesInScene;
        private readonly List<IUpdatable> Updatables;
        private bool Started = false;

        public Scene() : this($"Scene_{Time.EpochNow}") { }

        public Scene(string name) {
            Name = name;
            EntitiesInScene = new List<Entity>();
            Updatables = new List<IUpdatable>();
        }

        public Entity FindEntityInScene(string name) {
            foreach (Entity e in EntitiesInScene) {
                if (name == e.Name) return e;
                var result = e.FindChildByName(name, true);
                if (result) return result;
            }
            return null;
        }
        
        public void Start() {
            Started = true;
            int i = 0;
            for (; i < EntitiesInScene.Count; i++)
                EntitiesInScene[i].Start();
            for (i = 0; i < Updatables.Count; i++)
                Updatables[i].Start();
        }

        public void Render() {
            int i = 0;
            for (; i < EntitiesInScene.Count; i++)
                EntitiesInScene[i].Render();
            for (i = 0; i < Updatables.Count; i++)
                Updatables[i].Render();
        }

        public void Update() {
            int i = 0;
            for (; i < EntitiesInScene.Count; i++)
                EntitiesInScene[i].Update();
            for (i = 0; i < Updatables.Count; i++)
                Updatables[i].Update();
        }

        public void Add(Entity entity) {
            if (Started) entity.Start();
            EntitiesInScene.Add(entity);
        }

        public void Add(params Entity[] entities) {
            if (Started) {
                foreach (var e in entities) {
                    e.Start();
                }
            }
            EntitiesInScene.AddRange(entities);
        }

        public void Add(IEnumerable<Entity> entities) {
            if (Started) {
                foreach (var e in entities) {
                    e.Start();
                }
            }
            EntitiesInScene.AddRange(entities);
        }

        public void Add(IUpdatable updatable) => Updatables.Add(updatable);

        public void Add(params IUpdatable[] updatables) => Updatables.AddRange(updatables);

        public void Add(IEnumerable<IUpdatable> updatables) => Updatables.AddRange(updatables);

        public void Remove(Entity entity) {
            EntitiesInScene.Remove(entity);
            // entity.Dispose();
        }

        public void Dispose() => EntitiesInScene.ForEach(e => e.Dispose());
    }
}
