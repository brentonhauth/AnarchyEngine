using AnarchyEngine.Rendering;
using AnarchyEngine.Rendering.Mesh;
using AnarchyEngine.Rendering.Shaders;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.ECS.Components {
    public class MeshFilter : Component {
        public IMesh Mesh { get; set; }
        public Material Material { get; set; }
        public Shader Shader { get; set; }

        public MeshFilter() { }
        
        public MeshFilter(IMesh mesh) {
            Mesh = mesh;
        }

        public override void Start() {
            Material = Material ?? Material.Default;
            Material.Shader.Init();
            Mesh.Init();
            Mesh.VertexArray.InitWithShader(Material.Shader);
        }

        public override void Render() {
            Mesh.Render();
            /*if (Mesh.Texture != null) {
                Renderer.Push(Mesh.Texture);
                //Renderer.Push("texture0", 1);
            }*/
            Renderer.Push(Mesh.Shader, Mesh.VertexArray, Material, Entity.Transform);
            Renderer.Submit();
        }
    }
}
