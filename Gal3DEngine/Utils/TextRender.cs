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
        public static void BasicDrawText(Screen screen, string text , string fontUrl ,string fontDataUrl, Vector2 position)
        {
            Bitmap map = new Bitmap(fontUrl);
            int baseHeight = 20;
            position.Y = screen.Height -baseHeight - position.Y; //the Y is flipped
            List<String> values = File.ReadAllText(fontDataUrl).Split(new char[]{'\n',','}).ToList();
            int x = 0, y =0;
            int space = 5;
            for(int i = 0; i< text.Length; i++)
            {
                if (text[i] != ' ')
                {
                    int index = (int)(text[i]) - 48;
                    x = index % 12 * 40;
                    y = index / 12 * 40;
                    int width = int.Parse(values[values.IndexOf(values.First<string>(n => n.Contains("Char " + index + " Base Width")), 0) + 1]);
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
        public const string ARIAL = @"Resources\ArialFont.bmp";
        public const string ARIALFONTDATA = @"Resources\ArialFontData.csv";
    }
}
