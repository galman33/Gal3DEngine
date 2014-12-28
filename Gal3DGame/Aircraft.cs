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

        private Quaternion rotation;
        private Vector3 position;

        public Aircraft()
        {
            rotation = Quaternion.Identity;
            position = new Vector3();
        }

        public static void LoadContent()
        {
            aircrafModel = new Model("Resources/Blue Ship_3d.obj", "Resources/Ship_3d_texture.png");
        }

        public void Update()
        {
            rotation = rotation * Quaternion.FromAxisAngle(Vector3.UnitZ, (float) Time.DeltaTime);
            Vector3 forward = Vector3.Transform(Vector3.UnitY, rotation);
            Console.WriteLine(forward);
            position += forward * (float) Time.DeltaTime;
        }

        public void Render(Screen screen, Matrix4 projection, Matrix4 view)
        {
            ShaderPhong.world = Matrix4.CreateScale(0.001f) * /*Matrix4.CreateRotationX((float) Math.Sin(Time.TotalTime * 10)) * Matrix4.CreateRotationZ((float) Math.Sin(Time.TotalTime)) * */
                Matrix4.CreateTranslation(0, -0.1f, 0) *
                Matrix4.CreateFromQuaternion(rotation) * 
                Matrix4.CreateRotationY(-MathHelper.PiOver2) * Matrix4.CreateTranslation(position);
            ShaderPhong.view = view;
            ShaderPhong.projection = projection;

            ShaderPhong.lightDirection = (new Vector3(-1, -1, -1)).Normalized();
            ShaderPhong.ambientLight = 0.3f;

            ShaderPhong.ExtractData(aircrafModel);

            ShaderPhong.Render(screen);

            Gal3DEngine.Gizmos.AxisGizmo.Render(screen, ShaderPhong.world.ClearScale(), view, projection);
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

    }
}
