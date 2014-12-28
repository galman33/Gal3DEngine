using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Gizmos;
using OpenTK.Input;

namespace DanielFlappyGame
{
    public class FlapGame : Game
    {
        //private List<Model> models = new List<Model>();
        private List<Entity> Tunnels = new List<Entity>();
        private FlappyBird flappyflappy;

        Random rand = new Random();
        private Camera gameCam;
        Matrix4 projection;
        public FlapGame() : base(640, 480)
        {
          
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gameCam = new Camera();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 10.0f);
            flappyflappy = new FlappyBird(Matrix4.CreateTranslation(new Vector3(0, 0, -2)), Matrix4.Identity, Matrix4.CreateScale(0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png"), 0.1f, 0.01f);
           
                Tunnels.Add( new Entity(Matrix4.CreateTranslation(new Vector3(0, 1, -5)) , Matrix4.Identity, Matrix4.CreateScale(0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png")));
                Tunnels.Add(new Entity(Matrix4.CreateTranslation(new Vector3(0, -1, -5)), Matrix4.Identity, Matrix4.CreateScale(0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png")));

            ShaderPhong.projection = projection;
           
        }
        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == OpenTK.Input.Key.Space)
            {
                flappyflappy.Jump();
            }
        }

        private float vel =0 ;

        protected override void Update()
        {
            base.Update();
           
            flappyflappy.Update();
            foreach (Entity entity in Tunnels)
            {
                entity.Update();
            }
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            gameCam.Position.X = flappyflappy.Position.X;
            gameCam.Position.Y = flappyflappy.Position.Y;
            gameCam.Position.Z = flappyflappy.Position.Z + 3f;
        }
        protected override void Render()
        {
            base.Render();
            //DrawModel(projection, view);
            Matrix4 view = gameCam.GetViewMatrix();
            ShaderPhong.view = view;
            ShaderPhong.lightDirection = Vector3.Normalize(new Vector3(1, -0.25f, -1));
            AxisGizmo.Render(Screen, Matrix4.Identity, view, projection);
            flappyflappy.Render(Screen);
            foreach (Entity entity in Tunnels)
            {
                entity.Render(Screen);
            }
        }

        /*private void DrawModel(Matrix4 projection, Matrix4 view)
        {
            ShaderPhong.projection = projection;
            ShaderPhong.view = view;

            ShaderPhong.lightDirection = Vector3.Normalize(new Vector3(1, -0.25f, -1));

            for (int i = 0; i < 1; i++)
            {
                ShaderPhong.world = Matrix4.CreateRotationY((float)Time.TotalTime * 3) * Matrix4.CreateTranslation(0, 0, -3);
                ShaderPhong.ExtractData(models[i]);
                ShaderPhong.Render(Screen);

                AxisGizmo.Render(Screen, ShaderPhong.world, view, projection);
            }
        }*/

        public static Key pressed;
    }
}
