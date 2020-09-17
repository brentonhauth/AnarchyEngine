using AnarchyEngine.ECS.Components;
using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;

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
