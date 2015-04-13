using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Gal3DEngine.UTILS
{
    public class TextRender
    {
        private string fontPath, fontDataPath;
        List<String> charValues;
        List<int> widths;
        Bitmap map;


        public const int CharHeight = 32;
        public const int StartUnicode = 33;

        public TextRender(string fontUrl, string fontDataUrl)
        {
            this.fontPath = fontUrl;
            this.fontDataPath = fontDataUrl;
            LoadRender();
        }

        public void SetNewFont(string fontUrl, string fontDataUrl)
        {
            this.fontPath = fontUrl;
            this.fontDataPath = fontDataUrl;
            LoadRender();
        }

        private void LoadRender()
        {
            map = new Bitmap(this.fontPath);
            charValues = File.ReadAllText(this.fontDataPath).Split(new char[] { '\n', ',' }).ToList();
            widths = new List<int>();
            for (int i = 0; i < map.Width / CharHeight * map.Height / CharHeight; i++)
            {
                widths.Add(int.Parse(charValues[charValues.IndexOf(charValues.First<string>(n => n.Contains("Char " + i + " Base Width")), 0) + 1]));
            }
        }

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

    public static class Fonts
    {
        public const string ARIAL = @"Resources\ArialFontNew.png";
        public const string ARIALFONTDATA = @"Resources\ArialFontDataNew.csv";
    }
}
