using System.Drawing;
using System.Drawing.Imaging;
using AnarchyEngine.Util;
using OpenTK.Graphics.OpenGL4;
using SysPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using OpenTK;

namespace AnarchyEngine.Rendering {
    public class Texture : IPipable {
        public int Handle { get; private set; }

        private readonly string Path;
        private bool Initialized = false;

        public Texture() { }

        // Create texture from path.
        public Texture(string path) {
            Path = path;
            Renderer.ScheduleForInit += Init;
        }
        public void Init() {
            if (Initialized) return; else Initialized = true;

            Handle = GL.GenTexture();
            Use();
            using (var img = new Bitmap(FileHelper.Path + Path)) {
                var data = img.LockBits(
                    rect: new Rectangle(0, 0, img.Width, img.Height),
                    flags: ImageLockMode.ReadOnly,
                    format: SysPixelFormat.Format32bppArgb);
                
                GL.TexImage2D(
                    target: TextureTarget.Texture2D,
                    level: 0,
                    internalformat: PixelInternalFormat.Rgba,
                    width: img.Width,
                    height: img.Height,
                    border: 0,
                    format: PixelFormat.Bgra,
                    type: PixelType.UnsignedByte,
                    pixels: data.Scan0);
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use() => Use(TextureUnit.Texture0);

        public void Use(TextureUnit unit) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose() {
        }

        public static TextureUnit UnitFromInt(int i) {
            i = Maths.Clamp(i, 0, 31);
            return i + TextureUnit.Texture0;
        }

        public static int UnitToInt(TextureUnit unit) {
            return unit - TextureUnit.Texture0;
        }
    }
}
