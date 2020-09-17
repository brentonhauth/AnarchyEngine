using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnarchyEngine.Util;
using AnarchyEngine.DataTypes;
using OpenTK.Graphics;
using SysColor = System.Drawing.Color;

namespace AnarchyEngine.DataTypes {
    public struct Color {

        public float R, G, B, A;

        public Color(float r, float g, float b, float a) {
            R = r; G = g;
            B = b; A = a;
        }

        public Color(byte red, byte green, byte blue, float alpha) {
            R = G = B = A = 0f;
        }

        public static implicit operator Color(Color4 c) => new Color(c.R, c.G, c.B, c.A);
        public static implicit operator Color4(Color c) => new Color4(c.R, c.G, c.B, c.A);
        public static implicit operator Color(SysColor c) => new Color(c.R, c.G, c.B, c.A);
        public static implicit operator SysColor(Color c) => SysColor.FromArgb(
            red: Maths.Clamp((int)(c.R * 255), 0, 255),
            blue: Maths.Clamp((int)(c.B * 255), 0, 255),
            green: Maths.Clamp((int)(c.G * 255), 0, 255),
            alpha: Maths.Clamp((int)(c.A * 255), 0, 255));
    }
}
