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

        public MeshFilter() { }
        
        public MeshFilter(IMesh mesh) {
            Mesh = mesh;
        }

        public override void Start() {
            Mesh.MeshFilter = this;
            Mesh.Init();
        }

        public override void Render() {
            Mesh.Render();
            if (Mesh.Texture != null) {
                Renderer.Push(Mesh.Texture);
                //Renderer.Push("texture0", 1);
            }
            Renderer.Push(Mesh.Shader, Mesh.VertexArray, Entity.Transform);
            var color = Color4.Gold;// Color4.FromXyz( Vector4.One );
            Renderer.Push(Shader.ColorName, new OpenTK.Vector3(color.R, color.G, color.B));
            Renderer.Submit();
        }
    }
}
