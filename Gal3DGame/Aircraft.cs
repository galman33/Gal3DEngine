using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;

namespace Gal3DGame
{
    class Aircraft
    {
        private static Model aircrafModel;

        public static void LoadContent()
        {
            aircrafModel = new Model("Resources/Blue Ship_3d.obj", "Resources/Ship_3d_texture.png");
        }

        public void Render(Screen screen, Matrix4 projection, Matrix4 view)
        {
            ShaderPhong.world = Matrix4.CreateScale(0.001f) * Matrix4.CreateRotationX((float) Math.Sin(Time.TotalTime * 10)) * Matrix4.CreateRotationZ((float) Math.Sin(Time.TotalTime)) *
                Matrix4.CreateRotationY(-MathHelper.PiOver2);//* Matrix4.CreateRotationZ((float) Time.TotalTime);
            ShaderPhong.view = view;
            ShaderPhong.projection = projection;

            ShaderPhong.ExtractData(aircrafModel);

            ShaderPhong.lightDirection = (new Vector3(-1, -1, -1)).Normalized();

            ShaderPhong.Render(screen);

            Gal3DEngine.Gizmos.AxisGizmo.Render(screen, ShaderPhong.world.ClearScale(), view, projection);
        }
    }
}
