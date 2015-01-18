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
        private List<Entity> passedTunnles = new List<Entity>(); // the bird already passed them but the camera still sees them
        private FlappyBird flappyflappy;
        
        private Camera gameCam;
        Matrix4 projection;
        float rot = 0.1f;

        public FlapGame() : base(640, 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Program.world = this;
            points = 0;
            gameCam = new Camera();
            
            //gameCam.Position.X = 3;
            gameCam.Rotation.Y = 90;//MathHelper.Pi / 2.0f;
            gameCam.Position.X = 5.0f;
            gameCam.Position.Z = -5f;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 10.0f);

            flappyflappy = new FlappyBird(new Vector3(0, 0, -2), Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png"), 0.05f, 0.01f);

            Tunnels.Add(new Entity(new Vector3(0, 0.5f, -5f), Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png")));
            Tunnels.Add(new Entity(new Vector3(0, -0.5f, -5f), Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), new Model("Resources/Cat2.obj", "Resources/Cat2.png")));

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



        protected override void Update()
        {
            base.Update();

            //entities update
             flappyflappy.Update();
             foreach (Entity entity in Tunnels)
             {
                 entity.Update();
             }
             
   //          gameCam.Rotation.Y += rot;
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            //gameCam.Position.Z = Tunnels[0].Position.Z;
           // gameCam.Position.X = Tunnels[0].Position.X + 1f;
            
            //gameCam.Position.X = flappyflappy.Position.X;
            //gameCam.Position.Y = flappyflappy.Position.Y+0.5f;
            //gameCam.Position.Z = flappyflappy.Position.Z;
            //gameCam.Position.X = (float) Math.Sin(Time.TotalTime);
            //gameCam.Position.Z = (float)Math.Cos(Time.TotalTime);
        }
        protected override void Render()
        {
            base.Render();

            Matrix4 view = gameCam.GetViewMatrix();
            ShaderPhong.view = view;
            ShaderPhong.lightDirection = Vector3.Normalize(new Vector3(1, -0.25f, -1)); // basic light
            
            //AxisGizmo.Render(Screen, Matrix4.CreateTranslation(viewTrans.X, viewTrans.Y - 0.5f, viewTrans.Z-3), view, projection);
            DrawEntities();
           
            Entity.Cube.DrawCube(Screen, Matrix4.CreateTranslation(flappyflappy.Position), view, projection);
        }

        private void DrawEntities()
        {
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

        public List<Entity> GetTunnles()
        {
            return this.Tunnels;
        }

        public static Key pressed;

        public void GameOver()
        {
            gameOver = true;
        }
        private bool gameOver = false;

        public void PassedTunnles(Entity[] tunnles)
        {
            foreach (Entity tunnel in tunnles)
            {
                tunnles.ToList().Remove(tunnel);
                passedTunnles.Add(tunnel);
            }
        }

        public void AddPoints()
        {
            points++;
        }
        private int points;
    }
}
