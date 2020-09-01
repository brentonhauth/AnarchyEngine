using AnarchyEngine.ECS.Components;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering.Mesh {
    public interface IMesh {
        MeshFilter MeshFilter { get; set; }
        Shader Shader { get; }
        Texture Texture { get; }
        VertexArray VertexArray { get; }
        void Init();
        void Render();
    }
}
