﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Gal3DEngine.Utils
{
    /// <summary>
    /// Responisbles for rendering text into screen.
    /// </summary>
    public class TextRender
    {
        /// <summary>
        /// The text font path.
        /// </summary>
        private string fontPath;
        /// <summary>
        /// The text font csv data file path.
        /// </summary>
        private string fontDataPath;

        
        /// <summary>
        /// List containign the chars widths.
        /// </summary>
        List<int> widths;
        /// <summary>
        /// The image containg the letters.
        /// </summary>
        Bitmap map;

        /// <summary>
        /// The height of the letters.
        /// </summary>
        public const int CharHeight = 32;
        /// <summary>
        /// The start letter of the available Text.
        /// </summary>
        public const int StartUnicode = 33;

        /// <summary>
        /// Initiallize A renderer from given font (image) path and csv data file.
        /// </summary>
        /// <param name="fontUrl">The font (image) path.</param>
        /// <param name="fontDataUrl">The font csv data file.</param>
        public TextRender(string fontUrl, string fontDataUrl)
        {
            this.fontPath = fontUrl;
            this.fontDataPath = fontDataUrl;
            LoadRender();
        }

        /// <summary>
        /// Sets new font to the current TextRender
        /// </summary>
        /// <param name="fontUrl"></param>
        /// <param name="fontDataUrl"></param>
        public void SetNewFont(string fontUrl, string fontDataUrl)
        {
            this.fontPath = fontUrl;
            this.fontDataPath = fontDataUrl;
            LoadRender();
        }

        private void LoadRender()
        {
            map = new Bitmap(this.fontPath);
            List<String> charValues = File.ReadAllText(this.fontDataPath).Split(new char[] { '\n', ',' }).ToList();
            widths = new List<int>();
            for (int i = 0; i < map.Width / CharHeight * map.Height / CharHeight; i++)
            {
                widths.Add(int.Parse(charValues[charValues.IndexOf(charValues.First<string>(n => n.Contains("Char " + i + " Base Width")), 0) + 1]));
            }
        }

        /// <summary>
        /// Render a given text into a given screen , at a given position.
        /// </summary>
        /// <param name="screen">The screen to render the text on.</param>
        /// <param name="text">The text to render.</param>
        /// <param name="position">The position to render in.</param>
        public void RenderText(Screen screen, string text, Vector2 position)
        {
            int baseHeight = 20;
            position.Y = screen.Height - baseHeight - position.Y; //the Y is flipped
            int x = 0, y = 0;
            int space = 5;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ')
                {
                    int index = (int)(text[i]) - StartUnicode;
                    x = index % 16 * 32;
                    y = index / 16 * CharHeight;
                    int width = widths[index];
                    for (int a = 0; a < width; a++)
                    {
                        for (int b = 0; b < CharHeight; b++)
                        {
                            Color3 color = new Color3(map.GetPixel(a + x, CharHeight - b/*זה מתקן את הקטע שהוא הפוך*/ + y));
                            if (color != Color3.Transparent)
                                screen.PutPixel((int)position.X + a, (int)position.Y + b, color);
                        }
                    }

                    position.X += space + width;
                }
                else
                {
                    position.X += space + space;
                }
            }
        }
    }

    /// <summary>
    /// Helper class providing all fonts paths and csv data paths.
    /// </summary>
    public static class Fonts
    {
        public const string ARIAL = @"Resources\ArialFontNew.png";
        public const string ARIALFONTDATA = @"Resources\ArialFontDataNew.csv";
    }
}
