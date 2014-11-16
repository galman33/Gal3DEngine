using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Gal3DEngine
{
    class Texture
    {

        public static Color3[,] LoadTexture(string path)
        {
            Bitmap bmp = new Bitmap(path);
            Color3[,] pixels = new Color3[bmp.Width, bmp.Height];
            for(int x = 0; x < bmp.Width; x++)
            {
                for(int y = 0; y < bmp.Height; y++)
                {
                    pixels[x, y] = new Color3(bmp.GetPixel(x, y));
                }
            }
            return pixels;
        }

    }
}
