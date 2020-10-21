using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS {
    public class Entity : Core.Object {

        private static uint IdCount = 0;

        private static readonly List<Entity> AllEntities; // Remove (?)
        

        public readonly uint Id;

        private readonly List<Component> Components;

        private readonly List<Entity> Children;

        public string Name { get; set; }

        public Entity Parent { get; private set; }

        public Transform Transform { get; }
        
        internal EntityEvents Events { get; }


        static Entity() {
            AllEntities = new List<Entity>(0);
        }

        public Entity() : this($"Entity_{Time.EpochNow}") { }

        public Entity(string name) {
            Id = ++IdCount;
            Name = name;
            Components = new List<Component>();
            Children = new List<Entity>();
            Events = new EntityEvents();
            Transform = new Transform(this);
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
            Events.RaiseAddedComponent(component);
            Components.Add(component);
        }

        public T AddComponent<T>() where T : Component {
            if (typeof(T) == typeof(Component)) {
                throw new Exception();
            }
            T component = Activator.CreateInstance<T>();
            component.AppendTo(this);
            Components.Add(component);
            Events.RaiseAddedComponent(component);
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
            return child;
        }

        public IEnumerable<Entity> FlattenWithChildren() {
            var x = Children.AsParallel().SelectMany(e => {
                return e.FlattenWithChildren();
            }).ToList();
            x.Insert(0, this);
            return x;
        }

        public static Entity FindByName(string name) {
            return Scene.Current?.FindEntityInScene(name);
        }

        public override void Init() {
            base.Init();
            int i = 0;
            for (; i < Components.Count; i++)
                Components[i].Init();
            for (i = 0; i < Children.Count; i++)
                Children[i].Init();
        }

        public override void Start() {
            base.Start();
            int i = 0;
            for (; i < Components.Count; i++)
                Components[i].Start();
            for (i = 0; i < Children.Count; i++)
                Children[i].Start();
        }

        public override void Render() {
            int i = 0;
            for (; i < Components.Count; i++)
                Components[i].Render();
            for (i = 0; i < Children.Count; i++)
                Children[i].Render();
        }

        public override void Update() {
            int i = 0;
            for (; i < Components.Count; i++)
                Components[i].Update();
            for (i = 0; i < Children.Count; i++)
                Children[i].Update();

        }

        public void SetParent(Entity entity) {
            // ...
            Parent = entity;
        }

        public void AddChild(Entity entity) {
            entity.SetParent(this);
            Children.Add(entity);
        }

        public override void Dispose() {
            int i = 0;
            for (; i < Components.Count; i++)
                Components[i].Dispose();
            for (i = 0; i < Children.Count; i++)
                Children[i].Dispose();
        }

        public override string ToString() {
            return $"Entity: ({Id}, {Name})";
        }

        #region EntityEvents
        internal class EntityEvents {
            internal event Action<Component> AddedComponent;
            internal event Action<Vector3> UpdatePosition;
            internal event Action<Vector3> UpdateScale;
            internal event Action<OpenTK.Quaternion> UpdateRotation;

            internal EntityEvents() { }

            internal void RaiseAddedComponent(Component c) => AddedComponent?.Invoke(c);
            internal void RaiseUpdatePosition(ref Vector3 p) => UpdatePosition?.Invoke(p);
            internal void RaiseUpdateScale(ref Vector3 s) => UpdateScale?.Invoke(s);
            internal void RaiseUpdateRotation(ref OpenTK.Quaternion q) => UpdateRotation?.Invoke(q);
        }
        #endregion
    }

}
