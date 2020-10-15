using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnarchyEngine;
using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using AnarchyEngine.ECS.Components;
using AnarchyEngine.Rendering;
using AnarchyEngine.Rendering.Mesh;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using AnarchyEngine.Util;

namespace AnarchyRunner {
    class Program {
        private static float[] planeVertices = {
            // positions            // normals         // texcoords
             1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,   0.0f,  0.0f,
            -1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,

             1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,
             1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,  10.0f, 10.0f
        };

        static void Main(string[] args) {
            ThrowCubesDemo();
            /*int x = 2323;
            int y = 67;
            Utilities.Swap(ref x, ref y);
            Console.WriteLine($"x: {x}, y: {y}");*/
            Console.ReadKey();
        }

        private static void ThrowCubesDemo() {
            Scene scene = new Scene();
            List<Entity> entities = new List<Entity>();
            

            Mesh duckMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\Duck.fbx", VertexProperty.All),
                wolfMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\wolf.fbx", VertexProperty.All),
                planeMesh = Mesh.FromRaw(planeVertices);

            //var tex = new Texture(@"\Resources\img2.jpg");
            //planeMesh.AddTexture(tex);

            Entity planeEntity = new Entity();
            var planeFilter = planeEntity.AddComponent<MeshFilter>();
            planeFilter.Mesh = planeMesh;
            planeFilter.Material = Material.Default;
            planeEntity.Transform.Position = new Vector3(0f, -1f, 0f);
            planeEntity.Transform.Scale = new Vector3(50f, 1, 50f);
            planeEntity.AddComponent<EmptyRigidBody>();
            planeEntity.AddComponent<BoxCollider>().Size = planeEntity.Transform.Scale;
            scene.Add(planeEntity);

            //for (int i = 0; i < 15; i++) {
            Entity duckEntity = new Entity();
            duckEntity.AddComponent(new MeshFilter(duckMesh));
            duckEntity.Transform.Position = new Vector2(.5f, -.5f).Z0;
            duckEntity.Transform.Scale *= .15f;
                scene.Add(duckEntity);
                Entity wolfEntity = new Entity();
            wolfEntity.AddComponent(new MeshFilter(wolfMesh));
            wolfEntity.Transform.Position = -Vector3.UnitY * .5f;
            wolfEntity.Transform.Scale *= .01f;
            scene.Add(wolfEntity);

            Queue<Entity> cubes = new Queue<Entity>();

            scene.Add(new Updatable {
                OnUpdate = () => {
                    
                    if (Input.IsKeyPressed(Key.F)) {
                        var cam = Camera.Main;
                        var entity = AddBox((1.1f * cam.Front) + cam.Position, cam.Front * 5f);
                        scene.Add(entity);
                        cubes.Enqueue(entity);
                    }
                    if (Input.IsKeyPressed(Key.Q)) {
                        while (cubes.Count > 0) {
                            using (var cube = cubes.Dequeue()) {
                                scene.Remove(cube);
                            }
                        }
                    }
                }
            });

            World.AddScene(scene);
            World.Run("yee", 1000, 640);
        }


        private static Entity AddBox(in Vector3 position, in Vector3 velocity) {
            Entity entity = new Entity();
            var mf = entity.AddComponent<MeshFilter>();
            var rb = entity.AddComponent<RigidBody>();
            var bc = entity.AddComponent<BoxCollider>();
            mf.Mesh = Mesh.Cube;
            if (velocity != Vector3.Zero) {
                rb.IsBodyActive = true;
                rb.Velocity = velocity;
            } else {
                rb.IsBodyActive = false;
            }
            rb.Mass = 100f;
            rb.AffectedByGravity = true;
            entity.Transform.Position = position;
            return entity;
        }
    }
}
