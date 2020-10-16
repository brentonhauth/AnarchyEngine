using AnarchyEngine.Core;
using AnarchyEngine.Rendering;
using AnarchyEngine.Rendering.Mesh;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {
    public class MeshFilter : Component {
        public Mesh Mesh { get; set; }
        public Material Material { get; set; } = Material.Default;
        public Shader Shader { get; set; }

        public MeshFilter() : base() {
            Renderer.ScheduleForInit += Init;
        }
        
        public MeshFilter(Mesh mesh) : this() {
            Mesh = mesh;
        }

        public override void Init() {
            /// FIXME: Quick fix to correct "disappearing wolf" bug
            if (Initialized || Mesh.VertexArray != null) return;

            base.Init();
            Material = Material ?? Material.Default;
            Material.Shader.Init();
            Mesh.Init();
            Mesh.VertexArray.Bind(Material.Shader);
        }

        public override void Start() {
            base.Start();
            // VertexArray va = Mesh.VertexArray;
            // va.Bind(Material.Shader);
        }

        public override void Render() {
            Mesh.Render();
            /*if (Mesh.Texture != null) {
                Renderer.Push(Mesh.Texture);
                //Renderer.Push("texture0", 1);
            }*/
            if ((Time.Ticks % 90) == 0) {
                var data = Mesh.VertexArray.VertexBuffer.Data;
                //Entity.DebugCallIf("WOLF_ENTITY", $".MeshFilter.Render() -> {data.Length}");
            }

            Renderer.Push(Entity.Name, Material, Mesh.VertexArray, (OpenTK.Matrix4)Entity.Transform);
            Renderer.Submit();
        }
    }
}
