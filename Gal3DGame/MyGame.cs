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

        private List<Star> stars = new List<Star>();

        public MyGame() : base(500, 400)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Aircraft.LoadContent();
            Enviroment.LoadContent();
            Star.LoadContent();

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 20.0f);

            camera = new Camera();

            aircraft = new Aircraft();

            bear = new Model("Resources/bear.obj", "Resources/bear.jpg");

            enviroment = new Enviroment();

            AddStars();

            BackgroundColor = new Color3(128, 255, 255);

            ResetGame();
        }

        private void AddStars()
        {
            for (int i = 0; i < 20; i++)
            {
                stars.Add(new Star(enviroment, aircraft));
            }
        }

        private void ResetGame()
        {
            aircraft.Reset();
            camera.Rotation = Quaternion.Identity;
        }

        protected override void Update()
        {
            base.Update();
            aircraft.Update();

            camera.Rotation = Quaternion.Slerp(camera.Rotation, aircraft.Rotation, (float)Time.DeltaTime * 3.0f);
            camera.Position = aircraft.Position - Vector3.Transform(-Vector3.UnitZ, camera.Rotation).Normalized() * 2;

            if (enviroment.IsCollidingWith(aircraft.CollisionBox))
            {
                ResetGame();
            }
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

            enviroment.Render(Screen, view, projection);

            aircraft.Render(Screen, projection, view);

            RenderStars(view);
        }

        private void RenderStars(Matrix4 view)
        {
            for(int i = 0; i < stars.Count; i++)
            {
                stars[i].Render(Screen, projection, view);
            }
        }

        public static Random Random = new Random();

    }
}
