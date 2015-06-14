using Gal3DEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
       public static class ToolsHelper
       {
           public static void RenderImageRectangle(Screen screen , Color3[,] rec , Vector2 offset)
           {
               int baseHeight = 40;
               offset.Y = screen.Height - baseHeight - offset.Y;
               for (int a = 0; a < rec.GetLength(0); a++)
               {
                   for (int b = 0; b < rec.GetLength(1); b++)
                   {
                       screen.PutPixel((int)offset.X + a, (int)offset.Y + b, rec[a,b]);
                   }
               }
           }
       }
}
