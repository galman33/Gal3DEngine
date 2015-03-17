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
        Bitmap map;
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
                    int index = (int)(text[i]) - 33;
                    x = index % 16 * 32;
                    y = index / 16 * 32;
                    int width = int.Parse(charValues[charValues.IndexOf(charValues.First<string>(n => n.Contains("Char " + index + " Base Width")), 0) + 1]);
                    for (int a = 0; a < width; a++)
                    {
                        for (int b = 0; b < 40; b++)
                        {
                            screen.PutPixel((int)position.X + a, (int)position.Y + b, new Color3(map.GetPixel(a + x, 40 - b/*זה מתקן את הקטע שהוא הפוך*/ + y)));
                        }
                    }

                    position.X += space + width;
                }
                else
                {

                    for (int a = 0; a < space; a++)
                    {
                        for (int b = 0; b < 40; b++)
                        {
                            screen.PutPixel((int)position.X + a, (int)position.Y + b, new Color3(0, 0, 0));
                        }
                    }
                    position.X += space + space;
                }
            }
        }
    }

    public static class Fonts
    {
        public const string ARIAL = @"Resources\ArialFontNew.bmp";
        public const string ARIALFONTDATA = @"Resources\ArialFontDataNew.csv";
    }
}
