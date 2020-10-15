using System;
using System.Collections.Generic;
using AnarchyEngine.Rendering.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace AnarchyEngine.Rendering {
    public class Material {
        private const byte TextureLimit = 16;
        private const string TextureCountName = "uTextureCount";

        public static readonly Material Default = new Material();

        private Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
        private bool Initialized = false;
        public Shader Shader { get; set; } = Shader.Default;
        public Color4 Color { get; set; } = Color4.Transparent;

        // static Material() { }

        public Material() { }
        
        public Material(IDictionary<string, Texture> textures) {
            if (textures.Count > TextureLimit) {
                throw new ReachedTextureLimitException();
            }
            Textures = (Dictionary<string, Texture>)textures;
        }

        public void Init() {
            /*if (Initialized) return; else Initialized = true;
            foreach (var tex in Textures) {
                tex.Value.Init();
            }*/
        }

        public void SetTexture(string name, Texture texture) {
            if (Textures.Count == TextureLimit) {
                throw new ReachedTextureLimitException();
            }
            Textures[name] = texture;
        }

        internal void ApplyToShader(Shader shader) {
            ApplyTexturesToShader(shader);
            ApplyColorToShader(shader);
        }

        private void ApplyTexturesToShader(Shader shader) {
            shader.SetInt(TextureCountName, Textures.Count);

            if (Textures.Count == 0) return;
            
            var unit = TextureUnit.Texture0;
            foreach (var tex in Textures) {
                shader.SetInt(tex.Key, Texture.UnitToInt(unit));
                tex.Value.Use(unit);
                unit++;
            }
        }

        private void ApplyColorToShader(Shader shader) {
            var color = Color4.ToXyz(Color).Xyz.Normalized();
            shader.SetVector3(Shader.ColorName, color);
        }
    }

    public class ReachedTextureLimitException : Exception { }
}
