using AnarchyEngine.Core;
using Jitter.Collision;
using Jitter.LinearMath;
using JWorld = Jitter.World;
using JRigidBody = Jitter.Dynamics.RigidBody;
using AnarchyEngine.ECS.Components;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using System;

namespace AnarchyEngine.Physics {
    public static class Physics {
        internal static CollisionSystem CollisionSystem { get; private set; }
        internal static JWorld JWorld { get; private set; }

        internal static void Init() {
            CollisionSystem = new CollisionSystemSAP();
            JWorld = new JWorld(CollisionSystem);
            CoreECS.SubscribeComponentAdded<RigidBody>(AddRigidBody);
            CoreECS.SubscribeComponentEnabled<RigidBody>(AddRigidBody);
            CoreECS.SubscribeComponentRemoved<RigidBody>(RemoveRigidBody);
            CoreECS.SubscribeComponentDisabled<RigidBody>(RemoveRigidBody);
            CoreECS.SubscribeComponentAdded<EmptyRigidBody>(AddEmptyRigidBody);
            CoreECS.SubscribeComponentEnabled<EmptyRigidBody>(AddEmptyRigidBody);
            CoreECS.SubscribeComponentRemoved<EmptyRigidBody>(RemoveEmptyRigidBody);
            CoreECS.SubscribeComponentDisabled<EmptyRigidBody>(RemoveEmptyRigidBody);

            CollisionSystem.CollisionDetected += CollisionDetected;
        }

        private static void CollisionDetected(JRigidBody b0, JRigidBody b1, JVector p0, JVector p1, JVector normal, float depth) {
            if (b0.Tag is RigidBody r0) {
            }
            if (b1.Tag is RigidBody r1) {
            }
        }

        internal static void Update() {
            JWorld.Step(Time.DeltaTime, true);
            
            var phys = CoreECS.GetPhysicsRelated();
            foreach (var p in phys) {
                p.Update();
            }
        }


        internal static void Add(JRigidBody rb) => JWorld.AddBody(rb);

        internal static void Remove(JRigidBody rb) => JWorld.RemoveBody(rb);

        internal static void Dispose() {
            JWorld.Clear();
        }

        public static void AddEmptyRigidBody(in DefaultEcs.Entity Handle, in EmptyRigidBody rigidBody) {
            Add(rigidBody.Body);
        }

        public static void AddRigidBody(in DefaultEcs.Entity Handle, in RigidBody rigidBody) {
            Add(rigidBody.Body);
        }

        public static void RemoveEmptyRigidBody(in DefaultEcs.Entity Handle, in EmptyRigidBody rigidBody) {
            Remove(rigidBody.Body);
        }

        public static void RemoveRigidBody(in DefaultEcs.Entity Handle, in RigidBody rigidBody) {
            Remove(rigidBody.Body);
        }



        public static void Raycast(Ray ray) => Raycast(ray.Origin, ray.Direction);
        public static void Raycast(Vector3 origin, Vector3 direction) {
            CollisionSystem.Raycast(origin, direction, RaycastCallback, out var body, out var normal, out var fraction);
        }

        internal static bool RaycastCallback(JRigidBody body, JVector normal, float fraction) {
            return false;
        }
    }
}
