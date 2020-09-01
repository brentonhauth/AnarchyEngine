using AnarchyEngine.Core;
using Jitter.Collision;
using JWorld = Jitter.World;
using JRigidBody = Jitter.Dynamics.RigidBody;

namespace AnarchyEngine.Physics {
    internal static class PhysicsSystem {
        internal static CollisionSystem CollisionSystem { get; private set; }
        internal static JWorld JWorld { get; private set; }

        internal static void Init() {
            CollisionSystem = new CollisionSystemSAP();
            JWorld = new JWorld(CollisionSystem);
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
