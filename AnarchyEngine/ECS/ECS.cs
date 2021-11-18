using AnarchyEngine.ECS.Components;
using DefaultEcs;
using System;
using System.Collections.Generic;
using ECSWorld = DefaultEcs.World;

namespace AnarchyEngine.ECS {
    internal static class CoreECS {
        private static ECSWorld World;
        private static Dictionary<DefaultEcs.Entity, Entity> Entities;

        public static Entity GetWithHandle(DefaultEcs.Entity handle) => Entities[handle];

        public static void Init() {
            World = new ECSWorld();
            Entities = new Dictionary<DefaultEcs.Entity, Entity>();
        }

        public static Entity CreateEntity() {
            var handle = World.CreateEntity();
            var entity = new Entity(handle);
            return entity;
        }

        public static void Register(DefaultEcs.Entity handle, Entity entity) {
            if (entity) {
                Entities[handle] = entity;
            } else {
                Entities.Remove(handle);
            }
        }

        public static DefaultEcs.Entity CreateEntityHandle() {
            return World.CreateEntity();
        }

        public static Span<MeshFilter> GetRenderable() {
            return World.GetAll<MeshFilter>();
        }

        public static Span<RigidBody> GetPhysicsRelated() {
            return World.GetAll<RigidBody>();
        }

        public static void Subscribe<T>(MessageHandler<T> handler) {
            World.Subscribe(handler);
        }

        public static void SubscribeComponentAdded<T>(ComponentAddedHandler<T> handler) {
            World.SubscribeComponentAdded(handler);
        }

        public static void SubscribeComponentRemoved<T>(ComponentRemovedHandler<T> handler) {
            World.SubscribeComponentRemoved(handler);
        }

        public static void SubscribeComponentDisabled<T>(ComponentDisabledHandler<T> handler) {
            World.SubscribeComponentDisabled(handler);
        }

        public static void SubscribeComponentEnabled<T>(ComponentEnabledHandler<T> handler) {
            World.SubscribeComponentEnabled(handler);
        }
    }

    
}
