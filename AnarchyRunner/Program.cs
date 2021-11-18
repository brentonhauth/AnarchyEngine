using System;
using System.Collections.Generic;
using System.Diagnostics;
using AnarchyEngine.Core;
using AnarchyEngine.DataTypes;
using AnarchyEngine.ECS;
using AnarchyEngine.ECS.Components;
using AnarchyEngine.Rendering;
using AnarchyEngine.Rendering.Mesh;
using AnarchyEngine.Rendering.Vertices;
using AnarchyEngine.Util;

namespace AnarchyRunner {
    class Program : Application {
        private static float[] planeVertices = {
            // positions            // normals         // texcoords
             1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,   0.0f,  0.0f,
            -1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,

             1.0f, 0.5f,  1.0f,  0.0f, 1.0f, 0.0f,  10.0f,  0.0f,
            -1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,   0.0f, 10.0f,
             1.0f, 0.5f, -1.0f,  0.0f, 1.0f, 0.0f,  10.0f, 10.0f
        };

        private static Mesh wolfMesh;

        Program(GameSettings settings) : base(settings) { }

        static void Main(string[] args) {
            var settings = new GameSettings {
                Name = "Yee",
                Width = 1000,
                Height = 640,
                FPS = 60f,
            };

            var app = new Program(settings);
            app.Run();
        }

        private static void TestMatrixMultSpeed() {
            float r() => Maths.RandRange(0, 100);

            const int len = 100;

            var matrices = new Matrix4[len];
            for (int i = 0; i < len; ++i) {
                matrices[i] = new Matrix4(
                    r(), r(), r(), r(),
                    r(), r(), r(), r(),
                    r(), r(), r(), r(),
                    r(), r(), r(), r());
            }

            void t1(Matrix4[] mats) {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < len-1; ++i) {
                    Matrix4 result = mats[i] * mats[i + 1];
                }
                sw.Stop();
                Console.WriteLine($"Test1: {sw.Elapsed}");
            }

            void t2(Matrix4[] mats) {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < len-1; ++i) {
                    Matrix4.Multiply(in mats[i], in mats[i + 1], out Matrix4 result);
                }
                sw.Stop();
                Console.WriteLine($"Test2: {sw.Elapsed}\n");
            }

            t1(matrices);
            t2(matrices);

            Console.ReadKey();
        }

        
        public override void Setup() {
            Scene scene = new Scene();
            wolfMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\wolf.fbx", VertexProperty.All);

            
            Mesh planeMesh = Mesh.FromRaw(planeVertices);

            //var tex = new Texture(@"\Resources\img2.jpg");
            //planeMesh.AddTexture(tex);

            Entity planeEntity = new Entity();
            var planeTransform = planeEntity.Add<Transform>();
            var planeFilter = planeEntity.Add<MeshFilter>();
            planeEntity.Add<EmptyRigidBody>();
            planeFilter.Mesh = planeMesh;
            planeFilter.Material = Material.Default;
            planeTransform.Position = new Vector3(0f, -1f, 0f);
            planeTransform.Scale = new Vector3(50f, 1, 50f);
            
            planeEntity.Add<BoxCollider>().Size = planeTransform.Scale;
            SpawnWolf();
            SpawnDuck();


            Queue<Entity> cubes = new Queue<Entity>();
            
            scene.Add(new Updatable {
                OnUpdate = () => {
                    if (Input.IsKeyPressed(Key.F)) {
                        var cam = Camera.Main;
                        var entity = AddBox((1.1f * cam.Front) + cam.Position, cam.Front * 5f, null);
                        // scene.Add(entity);
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

            Entity cubeEnt = AddBox(Vector3.Zero, Vector3.Zero, null);// new Entity("CUBE_ENT");

            //scene.Add(cubeEnt);

            Vector2 _lastPos = Vector2.Zero;
            bool _firstMove = true;
            bool zoinks = true;

            scene.Add(new Updatable {
                OnUpdate = () => {
                    if (Input.IsKeyDown(Key.P)) {
                        zoinks = false;
                        Camera.Main.printtThing();
                    }

                    const float sensitivity = .3f;
                    var c = Camera.Main;

                    float cameraSpeed = (Input.IsKeyDown(Key.LeftCtrl) ? 4f : 2f) * Time.DeltaTime;


                    if (Input.IsKeyDown(Key.W))
                        c.Position += c.Front * cameraSpeed; // Forward 
                    if (Input.IsKeyDown(Key.S))
                        c.Position -= c.Front * cameraSpeed;
                    if (Input.IsKeyDown(Key.A))
                        c.Position -= c.Right * cameraSpeed;
                    if (Input.IsKeyDown(Key.D))
                        c.Position += c.Right * cameraSpeed;
                    if (Input.IsKeyDown(Key.Space))
                        c.Position += c.Up * cameraSpeed;
                    if (Input.IsKeyDown(Key.LeftShift))
                        c.Position -= c.Up * cameraSpeed;

                    var mouse = Input.MousePosition;

                    if (_firstMove) {
                        _lastPos = mouse;
                        _firstMove = false;
                    } else if (mouse != _lastPos) {
                        var delta = mouse - _lastPos;
                        c.Yaw += delta.X * sensitivity;
                        c.Pitch -= delta.Y * sensitivity;
                        _lastPos = mouse;
                    }
                }

            });

            World.AddScene(scene);
        }

        private static Entity SpawnDuck() {
            var duckMesh = Mesh.LoadFbx($@"{FileHelper.Path}\Resources\Duck.fbx", VertexProperty.All);
            Entity duckEntity = new Entity();
            var duckTransform = duckEntity.Add<Transform>();
            var filter = duckEntity.Add<MeshFilter>();
            filter.Mesh = duckMesh;
            filter.Material = new Material {
                Color = Color.Green
            };
            duckTransform.Position = new Vector2(.5f, -.5f).Z0;
            duckTransform.Scale *= .15f;
            return duckEntity;
        }

        private static Entity SpawnWolf(Mesh mesh=null) {
            Entity wolfEntity = new Entity();
            var wolfTransform = wolfEntity.Add<Transform>();
            var filter = wolfEntity.Add<MeshFilter>();
            filter.Mesh = mesh ?? wolfMesh;
            filter.Material = new Material {
                Color = Color.OrangeRed
            };
            wolfTransform.Position = -Vector3.UnitY * .5f;
            wolfTransform.Scale *= .01f;
            return wolfEntity;
        }

        private static Entity AddBox(in Vector3 position, in Vector3 velocity, Mesh mesh) {
            bool useCube = mesh == null;
            Entity entity = new Entity();
            var transform = entity.Add<Transform>();
            var mf = entity.Add<MeshFilter>();
            var rb = entity.Add<RigidBody>();
            var bc = entity.Add<BoxCollider>();
            mf.Mesh = mesh ?? Mesh.Cube;
            mf.Material = new Material {
                Color = Color.FireBrick
            };
            if (velocity != Vector3.Zero) {
                rb.IsBodyActive = true;
                rb.Velocity = velocity;
            } else {
                rb.IsBodyActive = false;
            }
            rb.Mass = 100f;
            rb.AffectedByGravity = true;
            transform.Scale *= useCube ? 1 : .01f;
            transform.Position = position;
            return entity;
        }
    }
}
