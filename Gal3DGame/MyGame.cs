using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Gizmos;

namespace Gal3DGame
{
    class MyGame : Game
    {

        private Aircraft aircraft;
        private Camera camera;
        private Matrix4 projection;

        private Model bear;
        private Enviroment enviroment;

        public MyGame() : base(640, 480)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Aircraft.LoadContent();
            Enviroment.LoadContent();

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 20.0f);

            camera = new Camera();
            camera.Position.Z = 4;

            aircraft = new Aircraft();

            bear = new Model("Resources/bear.obj", "Resources/bear.jpg");

            enviroment = new Enviroment();

            BackgroundColor = new Color3(128, 255, 255);
        }

        protected override void Update()
        {
            base.Update();
            aircraft.Update();

            camera.Rotation = Quaternion.Slerp(camera.Rotation, aircraft.Rotation, (float)Time.DeltaTime * 3.0f);
            camera.Position = aircraft.Position - Vector3.Transform(-Vector3.UnitZ, camera.Rotation).Normalized() * 2;
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            aircraft.KeyDown(e);
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            aircraft.KeyUp(e);
        }

        protected override void Render()
        {
            base.Render();

            Matrix4 view = camera.GetViewMatrix();

            AxisGizmo.Render(Screen, Matrix4.Identity, view, projection);

            RenderEnviroment(view);

            aircraft.Render(Screen, projection, view);
        }

        private void RenderEnviroment(Matrix4 view)
        {
            enviroment.Render(Screen, view, projection);

            /*ShaderPhong.world = Matrix4.Identity;
            ShaderPhong.view = view;
            ShaderPhong.projection = projection;

            ShaderPhong.lightDirection = (new Vector3(-1, -1, -1)).Normalized();
            ShaderPhong.ambientLight = 0.3f;

            ShaderPhong.ExtractData(bear);

            ShaderPhong.Render(Screen);*/
        }

    }
}
