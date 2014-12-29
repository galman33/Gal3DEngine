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

        private float rotateX;
        private float rotateY;

        public Aircraft()
        {
            rotation = Quaternion.Identity;
            position = new Vector3();

            rotateX = rotateY = 0;
        }

        public static void LoadContent()
        {
            aircrafModel = new Model("Resources/Blue Ship_3d.obj", "Resources/Ship_3d_texture.png");
        }

        public void Update()
        {
            rotation = rotation * Quaternion.FromAxisAngle(Vector3.UnitX, rotateX * (float) Time.DeltaTime);
            rotation = rotation * Quaternion.FromAxisAngle(Vector3.UnitY, rotateY * (float)Time.DeltaTime);

            //Console.WriteLine(rotateX);

            position += Forward * (float) Time.DeltaTime;
        }

        public void Render(Screen screen, Matrix4 projection, Matrix4 view)
        {
            ShaderPhong.world = Matrix4.CreateScale(0.001f) *
                Matrix4.CreateTranslation(0, -0.1f, 0) *
                Matrix4.CreateRotationY(-MathHelper.PiOver2) *
                Matrix4.CreateFromQuaternion(rotation) * 
                Matrix4.CreateTranslation(position);
            ShaderPhong.view = view;
            ShaderPhong.projection = projection;

            ShaderPhong.lightDirection = (new Vector3(-1, -1, -1)).Normalized();
            ShaderPhong.ambientLight = 0.3f;

            ShaderPhong.ExtractData(aircrafModel);

            ShaderPhong.Render(screen);

            Gal3DEngine.Gizmos.AxisGizmo.Render(screen, Matrix4.CreateFromQuaternion(rotation) *
                Matrix4.CreateTranslation(position), view, projection);
        }

        public void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Up)
                rotateX = 1;
            if (e.Key == OpenTK.Input.Key.Down)
                rotateX = -1;

            if (e.Key == OpenTK.Input.Key.Right)
                rotateY = -1;
            if (e.Key == OpenTK.Input.Key.Left)
                rotateY = 1;
        }

        public void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Up)
                rotateX = 0;
            if (e.Key == OpenTK.Input.Key.Down)
                rotateX = 0;

            if (e.Key == OpenTK.Input.Key.Right)
                rotateY = 0;
            if (e.Key == OpenTK.Input.Key.Left)
                rotateY = 0;
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

        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(-Vector3.UnitZ, rotation).Normalized();
            }
        }

    }
}
