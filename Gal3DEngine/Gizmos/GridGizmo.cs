using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine.Gizmos
{
    public class GridGizmo
    {
        public static void Render(Screen screen , Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            //X - red
            //Y - Green
            //Z - blue
           

            Tuple<Vector4 , Vector4>[,] grid = new Tuple<Vector4 , Vector4>[screen.Width/20 , screen.Height / 20];
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Vector4 top =new Vector4(x*20 ,  0 , 0, 1);
                    Vector4 buttom =  new Vector4(x*20 , screen.Height , 0 , 1);
                    Shader.TransformPosition(ref top, world * view * projection, screen);
                    Shader.TransformPosition(ref buttom, world * view * projection, screen);
                    //grid[x,y] = new Tuple<Vector4,Vector4>(top , buttom);
                    screen.DrawLine(top, buttom, new Color3(255, 0, 0));
                }
            }



        
        }

        private static bool ShouldRender(Vector4 p, Screen screen)
        {
            return p.X >= 0 && p.X < screen.Width && p.Y >= 0 && p.Y >= 0 && p.Y < screen.Height && p.Z > 0 && p.Z < 1;
        }

    }
}
