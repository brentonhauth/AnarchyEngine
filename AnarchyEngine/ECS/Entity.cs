using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using AnarchyEngine.Util;
using System.Runtime.CompilerServices;

namespace AnarchyEngine.ECS {
    [Flags]
    public enum EntityFlags {
        None = 0,
        HasRigidBody = 1,
        Renderable = 2,
    }

    public class Entity : IDisposable, IEquatable<Entity> {
        public bool IsAlive => Handle.IsAlive;
        
        internal DefaultEcs.Entity Handle { get; set; }

        internal EntityEvents Events { get; }

        public EntityFlags Flags { get; internal set; } = 0;

        internal Entity(DefaultEcs.Entity handle) {
            Handle = handle;
            CoreECS.Register(Handle, this);
            Events = new EntityEvents();
        }

        public Entity() : this(CoreECS.CreateEntityHandle()) { }

        ~Entity() {
            CoreECS.Register(Handle, null);
        }

        public T Get<T>() where T : Component {
            ComponentHelper.Assert<T>();
            return Handle.Get<T>();
        }

        public T Add<T>() where T : Component {
            ComponentHelper.Assert<T>();
            if (!ComponentHelper.AllowMultiple<T>() && Handle.Has<T>()) {
                throw new Exception($"Already has a component of type {typeof(T).Name}");
            }
            //if (typeof(T).Ba)
            T component = Activator.CreateInstance<T>();
            component.AppendTo(this);
            Events.RaiseAddedComponent(component);
            Handle.Set(component);
            return component;
        }

        public bool Has<T>() where T : Component {
            ComponentHelper.Assert<T>();
            return Handle.Has<T>();
        }
        public bool Has<T>(out T component) where T : Component {
            ComponentHelper.Assert<T>();
            component = Handle.Has<T>() ? Handle.Get<T>() : null;
            return component;
        }

        public bool IsEnabled() => Handle.IsEnabled();
        public bool IsEnabled<T>() where T : Component {
            ComponentHelper.Assert<T>();
            return Handle.IsEnabled<T>();
        }

        public void Enable() => Handle.Enable();
        public void Enable<T>() where T : Component {
            ComponentHelper.Assert<T>();
            Handle.Enable<T>();
        }

        public void Disable() => Handle.Disable();
        public void Disable<T>() where T : Component {
            ComponentHelper.Assert<T>();
            Handle.Disable<T>();
        }

        public void Dispose() {
            CoreECS.Register(Handle, null);
            Handle.Dispose();
        }

        public static bool operator ==(Entity left, Entity right) {
            return left.Handle == right.Handle;
        }
        public static bool operator !=(Entity left, Entity right) {
            return left?.Handle != right?.Handle;
        }

        public static implicit operator bool(Entity e) => e != null;

        public override bool Equals(object o) {
            return o is Entity e && Equals(e);
        }
        public bool Equals(Entity e) {
            return Handle == e.Handle;
        }

        public override int GetHashCode() {
            return 1786700523 + Handle.GetHashCode();
        }

        #region Entity Events
        internal class EntityEvents {
            internal event Action<Component> AddedComponent;
            internal event Action<Vector3> UpdatePosition;
            internal event Action<Vector3> UpdateScale;
            internal event Action<OpenTK.Quaternion> UpdateRotation;

            internal EntityEvents() { }

            internal void ClearAllEvents() {
                AddedComponent = null;
                UpdatePosition = null;
                UpdateScale = null;
                UpdateRotation = null;
            }

            internal void RaiseAddedComponent(Component c) => AddedComponent?.Invoke(c);
            internal void RaiseUpdatePosition(ref Vector3 p) => UpdatePosition?.Invoke(p);
            internal void RaiseUpdateScale(ref Vector3 s) => UpdateScale?.Invoke(s);
            internal void RaiseUpdateRotation(ref OpenTK.Quaternion q) => UpdateRotation?.Invoke(q);
        }
        #endregion
    }
}
