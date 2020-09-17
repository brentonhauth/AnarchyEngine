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
            //RunToTestBasicRigidBodies();
            ThrowCubesDemo();
        }

        private static void RunToTestBasicRigidBodies() {
            Scene scene = new Scene();
            List<Entity> entities = new List<Entity>();
            const float bounds = 5f;


            Mesh duckMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\Duck.fbx", VertexProperty.All),
                wolfMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\wolf.fbx", VertexProperty.All),
                planeMesh = Mesh.FromRaw(planeVertices).AddShaderWithVA(Shader.Default);
            
            var tex = new Texture(@"\Resources\img2.jpg");
            wolfMesh.AddTexture(tex);
            duckMesh.AddTexture(tex);
            planeMesh.AddTexture(tex);

            Entity planeEntity = new Entity();
            planeEntity.AddComponent(new MeshFilter(planeMesh));
            planeEntity.Transform.Position = new Vector3(0f, -1f, 0f);
            planeEntity.Transform.Scale = new Vector3(50f, 1, 50f);
            planeEntity.AddComponent<EmptyRigidBody>();
            var planeBC = planeEntity.AddComponent<BoxCollider>();
            planeBC.Size = planeEntity.Transform.Scale;
            scene.Add(planeEntity);

            for (int i = 0; i < 15; i++) {
                Entity entity = new Entity();
                entity.AddComponent(new MeshFilter(duckMesh));
                entity.Transform.Position = Vector3.Random(bounds, 2 * bounds);
                // entity.Transform.Rotation = new Quaternion(Vector3.Random(-bounds, bounds));
                entity.Transform.Scale *= .1f;
                entity.AddComponent<RigidBody>();
                scene.Add(entity);
            }

            for (int i = 0; i < 15; i++) {
                Entity entity = new Entity();
                entity.AddComponent(new MeshFilter(wolfMesh));
                entity.Transform.Position = Vector3.Random(bounds, 2 * bounds);
                // entity.Transform.Rotation = new Quaternion(Vector3.Random(-bounds, bounds));
                entity.Transform.Scale *= .01f;
                var rb = entity.AddComponent<RigidBody>();
                if (i == 0) {
                    rb.AffectedByGravity = true;
                }
                rb.IsBodyActive = true;
                scene.Add(entity);
            }

            Entity testEntity = new Entity();
            var testrb = testEntity.AddComponent<RigidBody>();
            testrb.IsBodyActive = true;
            scene.Add(testEntity);
            scene.Add(new Updatable {
                OnUpdate = () => {

                    testEntity.Transform.Position = World.MainCamera.Position;
                    
                }
            });

            World.AddScene(scene);
            World.Run("yee", 1000, 640);
        }

        private static void ThrowCubesDemo() {
            Scene scene = new Scene();
            List<Entity> entities = new List<Entity>();
            

            Mesh duckMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\Duck.fbx", VertexProperty.All),
                wolfMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\wolf.fbx", VertexProperty.All),
                planeMesh = Mesh.FromRaw(planeVertices).AddShaderWithVA(Shader.Default);

            //var tex = new Texture(@"\Resources\img2.jpg");
            //planeMesh.AddTexture(tex);

            Entity planeEntity = new Entity();
            planeEntity.AddComponent(new MeshFilter(planeMesh));
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
                        var cam = World.MainCamera;
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
