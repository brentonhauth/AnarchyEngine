using AnarchyEngine.Rendering.Shaders;
using AnarchyEngine.Rendering.Vertices;
using OpenTK;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnarchyEngine.Rendering {
    internal struct RenderContext {
        public Matrix4 Transform;
        public VertexArray VertexArray;
        public Material Material;
        // public Dictionary<string, RenderContextExtra> Extras;
    }

    internal struct RenderContextExtra {
        public ActiveUniformType Type;
        public dynamic Data;

        public RenderContextExtra(ActiveUniformType type, dynamic data) {
            Type = type;
            Data = data;
        }

        public static RenderContextExtra Int(int data) =>
            new RenderContextExtra(ActiveUniformType.Int, data);

        public static RenderContextExtra Float(float data) =>
            new RenderContextExtra(ActiveUniformType.Float, data);

        public static RenderContextExtra Vec3(Vector3 data) =>
            new RenderContextExtra(ActiveUniformType.FloatVec3, data);

        public static RenderContextExtra Mat4(Matrix4 data) =>
            new RenderContextExtra(ActiveUniformType.FloatMat4, data);
    }
}
