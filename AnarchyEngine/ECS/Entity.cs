using AnarchyEngine.Core;
using AnarchyEngine.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS {
    public class Entity : IDisposable {

        private static uint IdCount = 0;

        private static List<Entity> AllEntities; // Remove (?)


        public readonly uint Id;

        private List<Component> Components { get; }

        private List<Entity> Children { get; }

        public string Name { get; set; }

        public Entity Parent { get; private set; }

        public Transform Transform { get; }

        static Entity() {
            AllEntities = new List<Entity>(0);
        }

        public Entity() : this($"Entity_{Time.EpochNow}") { }

        public Entity(string name) {
            Id = ++IdCount;
            Name = name;
            Components = new List<Component>();
            Children = new List<Entity>();
            Transform = new Transform();
            AllEntities.Add(this);
        }

        public T GetComponent<T>() where T : Component {
            return Components.AsParallel().FirstOrDefault(c => c is T) as T;
        }

        // Remove (?)
        public void AddComponent(Component component) {
            Type type = component.GetType();
            if (component is ISingleComponent) {
                foreach (Component comp in Components) {
                    if (type.IsInstanceOfType(comp)) {
                        throw new Exception();
                    }
                }
            }
            component.Entity = this;
            component.AppendTo(this);
            Components.Add(component);
        }

        public T AddComponent<T>() where T : Component {

            //Type type = typeof(T);
            /*if (T is ISingleComponent) {
                foreach (Component comp in Components) {
                    if (type.IsInstanceOfType(comp)) {
                        throw new Exception();
                    }
                }
            }*/
            T component = Activator.CreateInstance<T>();
            component.AppendTo(this);
            Components.Add(component);
            return component;
        }

        public Entity FindChildByName(string name, bool deep = false) {
            if (!deep) {
                return Children.AsParallel().FirstOrDefault(e => e.Name == name);
            }

            foreach (Entity entity in Children) {
                if (entity.Name == name) {
                    return entity;
                }
                var result = entity.FindChildByName(name, true);

                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        public bool HasChildWithName(string name, out Entity child, bool deep = false) {
            child = FindChildByName(name, deep);
            return child != null;
        }

        public IEnumerable<Entity> FlattenWithChildren() {
            var x = Children.AsParallel().SelectMany(e => {
                return e.FlattenWithChildren();
            }).ToList();
            x.Insert(0, this);
            return x;
        }

        public static Entity FindByName(string name) {
            if (World.CurrentScene == null) {
                return null;
            }
            var scene = World.CurrentScene;
            return scene.FindEntityInScene(name);
        }

        public virtual void Start() {
            Components.ForEach(c => c.Start());
            Children.ForEach(e => e.Start());
        }

        public virtual void Render() {
            Components.ForEach(c => c.Render());
            Children.ForEach(e => e.Render());
        }

        public virtual void Update() {
            Components.ForEach(c => c.Update());
            Children.ForEach(e => e.Update());
        }

        public void SetParent(Entity entity) {
            // ...
            Parent = entity;
        }

        public void AddChild(Entity entity) {
            entity.SetParent(this);
            Children.Add(entity);
        }

        public void Dispose() {
            Components.ForEach(c => c.Dispose());
            Children.ForEach(e => e.Dispose());
        }

        public static implicit operator bool(Entity entity) => entity != null;
    }
}
