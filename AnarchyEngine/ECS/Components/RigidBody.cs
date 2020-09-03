using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using AnarchyEngine.Util;
using AnarchyEngine.Physics;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using Jitter;
using Jitter.LinearMath;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using JRigidBody = Jitter.Dynamics.RigidBody;

namespace AnarchyEngine.ECS.Components {
    public class RigidBody : Component {
        private static DebugDrawer drawer = new DebugDrawer();

        private static readonly BoxShape TestingPlaneShape = new BoxShape(new JVector(50, 1, 50));
        private static readonly BoxShape EmptyShape = new BoxShape(JVector.Zero);

        private JRigidBody Body;

        private bool IsPlane = false;

        public float Mass {
            get => Body.Mass;
            set => Body.Mass = value;
        }

        public bool IsStatic {
            get => Body.IsStatic;
            set => Body.IsStatic = value;
        }


        public RigidBody(float mass, bool isPlane = false) : base() {
            //var c = Entity?.GetComponent<Collider>();
            var s = // c?.Shape ?? 
                (IsPlane ? TestingPlaneShape : new BoxShape(JVector.One));
            Body = new JRigidBody(s);
            Mass = mass;
            IsPlane = isPlane;
        }

        public RigidBody() : this(1f) { }

        ~RigidBody() => Dispose();

        public override void Start() {
            drawer.Init();

            var transform = Entity.Transform;
            var rotation = transform.Rotation;

            Body.Material = new Material {
                KineticFriction = Maths.RandRange(.5f),
                Restitution = Maths.RandRange(.5f),
                StaticFriction = Maths.RandRange(.5f)
            };

            

            DataTypes.Matrix4 orientation = Matrix4.CreateFromQuaternion(rotation);

            Body.Orientation = (DataTypes.Matrix3)orientation;
            Body.Position =// IsPlane ? JVector.One * -1.5f :
                transform.Position;
            //Body.DebugDraw(drawer);
            //Body.EnableDebugDraw = true;
            Body.Mass = Mass;
            IsStatic = IsPlane;
            PhysicsSystem.Add(Body);
        }

        private void TransformListener(Transform.UpdateFactor factor) {
            const Transform.UpdateFactor
                pos = Transform.UpdateFactor.Position,
                rot = Transform.UpdateFactor.Rotation,
                scl = Transform.UpdateFactor.Scale;

            if ((factor & pos) == pos) {

            }

            if ((factor & rot) == rot) {

            }

            if ((factor & scl) == scl) {

            }
        }

        public override void Render() {
            DataTypes.Matrix3 rotation = Body.Orientation;
            DataTypes.Vector3 position = Body.Position;

            var model = new Matrix4(rotation)
                * Matrix4.CreateTranslation(position);
            drawer.Draw(model);
        }

        public override void Update() {
            if (IsStatic) return;

            DataTypes.Vector3 position = Body.Position;
            DataTypes.Matrix3 rotation = Body.Orientation;

            var transform = Entity.Transform;

            transform.SetPositionSilently(position);
            transform.SetRotationSilently(Quaternion.FromMatrix(rotation));
        }

        public override void AppendTo(Entity e) {
            e.Transform.OnUpdate += TransformListener;
            base.AppendTo(e);
        }

        public override void Dispose() {
            Entity.Transform.OnUpdate -= TransformListener;
            if (Body != null) {
                PhysicsSystem.Remove(Body);
            }
        }
    }

    public class EmptyRigidBody : RigidBody {
        public EmptyRigidBody() : base(0f) { }

        public override void Update() { }
    }

    public class DebugDrawer {
        Shader Shader;
        VertexArray VertexArray;
        private bool initialized = false;

        public DebugDrawer() {
            Shader = Shader.Flat;
            VertexArray = new VertexArray(Shader);
            VertexArray.Model("aPosition", 3, 3, 0);
        }

        public void Init() {
            if (initialized) return; else initialized = true;

            //VertexArray.AddData(Program.cubeVertices);

            Shader.Init();
            VertexArray.Init();


        }

        public void Draw(Matrix4 model) {
            VertexArray.Bind();
            Shader.Use();
            Shader.SetVector3(Shader.ColorName, new Vector3(1, 1, 0));
            Shader.SetMatrix4(Shader.ViewProjectionName, Core.World.MainCamera.ViewProjection);
            Shader.SetMatrix4(Shader.ModelName, model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        public void DrawLine(JVector start, JVector end) {
            VertexArray.Bind();
            Shader.Use();
            Shader.SetVector3(Shader.ColorName, Vector3.Zero);
            Shader.SetMatrix4(Shader.ViewProjectionName, Core.World.MainCamera.ViewProjection);
            Shader.SetMatrix4(Shader.ModelName, Matrix4.CreateTranslation((DataTypes.Vector3)(end - start)));
            Console.WriteLine("DrawLines");
            GL.DrawArrays(PrimitiveType.Lines, 0, 24);


            /*GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Color3(255, 255, 0);
            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(1000);
            GL.ShadeModel(ShadingModel.Flat);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(start.X, start.Y, start.Z);
            GL.Vertex3(end.X, end.Y, end.Z);
            GL.End();*/
        }

        /*public void DrawPoint(JVector pos) {
            
            VertexArray.Bind();
            Shader.Use();
            Shader.SetVector3(Shader.ColorName, Color4.Black.ToVector3());
            Shader.SetMatrix4(Shader.ViewProjectionName, Core.World.MainCamera.ViewProjection);
            Shader.SetMatrix4(Shader.ModelName, Matrix4.CreateTranslation(pos.ToOpenTK()));
            Console.WriteLine("DrawPoints");
            GL.DrawArrays(PrimitiveType.Points, 0, 12);
        }*/

        public void DrawTriangle(JVector v0, JVector v1, JVector v2) {
            VertexArray.Bind();
            Shader.Use();
            //Shader.SetVector3(Shader.ColorName, Color4.Black.ToVector3());
            Shader.SetMatrix4(Shader.ViewProjectionName, Core.World.MainCamera.ViewProjection);

            //Console.WriteLine("DrawTriangles");
            //Matrix4.CreateTranslation(((v0 + v1 + v2) * (1f / 3f)).ToOpenTK())
            Shader.SetMatrix4(Shader.ModelName, Matrix4.Identity);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            /*const bool solid = false;
            float[] u = new float[3], v = new float[3], normal = new float[3];
            u[0] = (float)(v1.X - v0.X);
            u[1] = (float)(v1.Y - v0.Y);
            u[2] = (float)(v1.Z - v0.Z);
            v[0] = (float)(v2.X - v0.X);
            v[1] = (float)(v2.Y - v0.Y);
            v[2] = (float)(v2.Z - v0.Z);

            //OdeMath.dCROSS(normal, CsODE.OP.EQ, u, v);
            normal[0] = u[1] * v[2] - u[2] - v[1];
            normal[1] = u[2] * v[0] - u[0] - v[2];
            normal[2] = u[0] * v[1] - u[1] - v[0];

            float len = v[0] * v[0] + v[1] * v[1] + v[2] * v[2];
            if (len <= 0.0f) {
                v[0] = 1;
                v[1] = 0;
                v[2] = 0;
            } else {
                len = 1.0f / (float)Math.Sqrt(len);
                v[0] *= len;
                v[1] *= len;
                v[2] *= len;
            }

            GL.Begin(solid ? BeginMode.Triangles : BeginMode.LineStrip);
            GL.Normal3(normal[0], normal[1], normal[2]);
            GL.Vertex3(v0.X, v0.Y, v0.Z);
            GL.Vertex3(v1.X, v1.Y, v1.Z);
            GL.Vertex3(v2.X, v2.Y, v2.Z);
            GL.End();*/
        }
    }
}
