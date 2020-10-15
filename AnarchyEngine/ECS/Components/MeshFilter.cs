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
            Material = Material ?? Material.Default;

            Mesh.Init();
            Mesh.VertexArray.Bind(Material.Shader);
        }

        public override void Start() {
            //VertexArray va = Mesh.VertexArray;
            // va.Bind(Material.Shader);
        }

        public override void Render() {
            Mesh.Render();
            /*if (Mesh.Texture != null) {
                Renderer.Push(Mesh.Texture);
                //Renderer.Push("texture0", 1);
            }*/
            VertexArray va = (Mesh as Mesh)?.VertexArray ?? null;
            

            Renderer.Push(Mesh.Shader, va, Material, Entity.Transform);
            Renderer.Submit();
        }
    }
}
