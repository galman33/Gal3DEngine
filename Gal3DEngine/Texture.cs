using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Gal3DEngine
{
    /// <summary>
    /// Helper class for loading image from the File System.
    /// </summary>
    public static class Texture
    {
        /// <summary>
        /// Load a texture from a specific path into a 2D pixel array.
        /// </summary>
        /// <param name="path">The path of the texture image</param>
        /// <returns>2D array of pixels</returns>
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
