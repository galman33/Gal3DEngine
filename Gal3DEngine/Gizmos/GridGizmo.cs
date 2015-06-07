using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine.Gizmos
{
    /// <summary>
    /// Helper class for rendering a grid gizmo.
    /// </summary>
    public static class GridGizmo
    {
        /// <summary>
        /// Render the grid gizmo with spefic world, view and projection matrices on a given Screen.
        /// </summary>
        /// <param name="screen">The screen to render on.</param>
        /// <param name="world">The world matrix of the grid gizmo.</param>
        /// <param name="view">The view matrix of the grid gizmo.</param>
        /// <param name="projection">The projection matrix of the grid gizmo.</param>
        public static void Render(Screen screen , Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            //X - red
            //Y - Green
            //Z - blue            
            for (int x = 0; x < screen.Width / 20; x++)
            {
                for (int y = 0; y < screen.Height / 20; y++)
                {
                    Vector4 top =new Vector4(x*20 ,  0 , 0, 1);
                    Vector4 buttom =  new Vector4(x*20 , screen.Height , 0 , 1);
                    ShaderHelper.TransformPosition(ref top, world * view * projection, screen);
                    ShaderHelper.TransformPosition(ref buttom, world * view * projection, screen);
                    
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
