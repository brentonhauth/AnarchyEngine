using AnarchyEngine.Core;
using Jitter.Collision;
using Jitter.LinearMath;
using JWorld = Jitter.World;
using JRigidBody = Jitter.Dynamics.RigidBody;
using AnarchyEngine.ECS.Components;

namespace AnarchyEngine.Physics {
    internal static class PhysicsSystem {
        internal static CollisionSystem CollisionSystem { get; private set; }
        internal static JWorld JWorld { get; private set; }

        internal static void Init() {
            CollisionSystem = new CollisionSystemSAP();
            JWorld = new JWorld(CollisionSystem);
            CollisionSystem.CollisionDetected += CollisionDetected;
        }

        private static void CollisionDetected(JRigidBody b0, JRigidBody b1, JVector p0, JVector p1, JVector normal, float depth) {
            if (b0.Tag is RigidBody r0) {
            }
            if (b1.Tag is RigidBody r1) {
            }
        }

        internal static void Update() {
            JWorld.Step((float)Time.DeltaTime, true);
        }

        internal static void Add(JRigidBody rb) => JWorld.AddBody(rb);

        internal static void Remove(JRigidBody rb) => JWorld.RemoveBody(rb);

        internal static void Dispose() {
            JWorld.Clear();
        }
    }
}
