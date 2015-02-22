using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine.Gizmos
{
    public class AxisGizmo
    {
        public static void Render(Screen screen , Matrix4 world, Matrix4 view, Matrix4 projection)
        {
            //X - red
            //Y - Green
            //Z - blue
            Vector4 origin = new Vector4(0, 0, 0, 1);
            Vector4 unitX = new Vector4(1, 0, 0, 1);
            Vector4 unitY = new Vector4(0, 1, 0, 1);
            Vector4 unitZ = new Vector4(0, 0, 1, 1);
           
            ShaderHelper.TransformPosition(ref unitX , world* view * projection , screen);
            ShaderHelper.TransformPosition(ref unitY, world * view * projection, screen);
            ShaderHelper.TransformPosition(ref unitZ, world * view * projection, screen);
            ShaderHelper.TransformPosition(ref origin, world * view * projection, screen);

            if (ShouldRender(origin, screen))
            {
                if (ShouldRender(unitX, screen))
                {
                    screen.DrawLine(origin, unitX, new Color3(255, 0, 0));
                }

                if (ShouldRender(unitY, screen))
                {
                    screen.DrawLine(origin, unitY, new Color3(0, 255, 0));
                }

                if (ShouldRender(unitZ, screen))
                {
                    screen.DrawLine(origin, unitZ, new Color3(0, 0, 255));
                }
            }
        }

        private static bool ShouldRender(Vector4 p, Screen screen)
        {
            return p.X >= 0 && p.X < screen.Width && p.Y >= 0 && p.Y >= 0 && p.Y < screen.Height && p.Z > 0 && p.Z < 1;
        }

    }
}
