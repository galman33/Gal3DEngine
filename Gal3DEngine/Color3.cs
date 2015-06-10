using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    /// <summary>
    /// Represent a RGB color.
    /// </summary>
    public struct Color3
    {
        /// <summary>
        /// The Red component of the color.
        /// </summary>
        public byte r;
        /// <summary>
        /// The Green component of the color.
        /// </summary>
        public byte g;
        /// <summary>
        /// The Blue component of the color.
        /// </summary>
        public byte b;

        /// <summary>
        /// Initiallize color from RGB
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public Color3(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color3(int r, int g, int b)
        {
            this.r = Convert.ToByte(r);
            this.g = Convert.ToByte(g);
            this.b = Convert.ToByte(b);
        }

        public Color3(System.Drawing.Color color)
        {
            if (color.A != 0)
            {
                this.r = color.R;
                this.g = color.G;
                this.b = color.B;
            }
            else
            {
                this.r = Transparent.r;
                this.g = Transparent.g;
                this.b = Transparent.b;
            }
        }

        /// <summary>
        /// Checks whether a Color is equal to another.
        /// </summary>
        /// <param name="obj">The compared Color.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Color3)
            {
                Color3 col = (Color3)obj;
                return r == col.r && g == col.g && b == col.b;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns a string representing the Color
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Color:{R: " + r + ", G:" + g + ", B:" + b + "}";
        }

        public static bool operator ==(Color3 color1, Color3 color2)
        {
            return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b;
        }

        public static bool operator !=(Color3 color1, Color3 color2)
        {
            return color1.r != color2.r || color1.g != color2.g || color1.b != color2.b;
        }
        /// <summary>
        /// The Transparent Color definition.
        /// </summary>
        public static Color3 Transparent = new Color3(255, 0, 255);

    }
}
