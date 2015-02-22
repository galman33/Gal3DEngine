using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Gizmos;
using OpenTK.Input;
using Gal3DEngine.UTILS;

namespace DanielFlappyGame
{
    public class FlapGame : Game
    {       
        private List<Entity> Tunnels = new List<Entity>();
        private List<Entity> passedTunnles = new List<Entity>(); // the bird already passed them but the camera still sees them
        public FlappyBird flappyflappy;
        public Entity Floor;

        private TimeSpan delta = new TimeSpan(0, 0, 0, 1, 500);
        private TimeSpan curDelta = TimeSpan.Zero;
        
        private Camera gameCam;
        Matrix4 projection;
        float rot = 0.1f;

        Model catModel;
        Model flapModel;
        Model floor;

        public FlapGame() : base(640, 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Program.world = this;
            rand = new Random();
            catModel = new Model("Resources/Cat2.obj", "Resources/Cat2.png");
            flapModel = new Model("Resources/Cat2.obj", "Resources/Cat2.png");
            floor = new Model("Resources/CobbleStones2.obj", "Resources/BrickRound0108_5_S.jpg");
            Init();
        }

        private void Init()
        {
            points = 0;
            gameCam = new Camera();

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 10.0f);

            flappyflappy = new FlappyBird(new Vector3(0, 0, -2), Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), flapModel, 0.05f, 0.01f);
            Floor = new Entity(new Vector3(-1, -1, 0), Vector3.One, Vector3.One, floor);

            //clear all tunnles in the game for game to start
            Tunnels.Clear();
            passedTunnles.Clear();

            Vector3[] positions = GetRandomPoint(-5f + flappyflappy.Position.Z);
            Tunnels.Add(new Entity(positions[0], Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), catModel));
            Tunnels.Add(new Entity(positions[1], Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), catModel));
            Tunnels[Tunnels.Count - 1].DestroyEntity += EntityDestroyed;
            Tunnels[Tunnels.Count - 2].DestroyEntity += EntityDestroyed;

            AvailableShaders.ShaderPhong.projection = projection;
        }
        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == OpenTK.Input.Key.Space)
            {
                flappyflappy.Jump();
            }
            if (e.Key == Key.Z)
            {
                front = !front;
                stateChange = true;
            }
            if(e.Key == Key.R)
            {
                Init();
            }
        }


        
        protected override void Update()
        {
            base.Update();

             // update entities
             flappyflappy.Update();
             foreach (Entity entity in Tunnels)
             {
                 entity.Update();
             }
             ChangeStateOfCamara();

            //tunnles update logic
            curDelta = curDelta.Add(TimeSpan.FromMilliseconds(16/4));
            if(curDelta.TotalMilliseconds > delta.TotalMilliseconds)//Math.Abs(this.flappyflappy.Position.Z - this.Tunnels[Tunnels.Count-1].Position.Z) <3) //curDelta.TotalMilliseconds > delta.TotalMilliseconds)
            {
                curDelta = TimeSpan.Zero;

                Vector3[] positions = GetRandomPoint(-5f + flappyflappy.Position.Z);
                if (recycleTunnels.Count ==0)
                {
                    Tunnels.Add(new Entity(positions[0], Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), catModel));
                    Tunnels.Add(new Entity(positions[1], Vector3.Zero, new Vector3(0.5f, 0.5f, 0.5f), catModel));
                    Tunnels[Tunnels.Count - 1].DestroyEntity += EntityDestroyed;
                    Tunnels[Tunnels.Count - 2].DestroyEntity += EntityDestroyed;
                }
                else
                {
                    recycleTunnels[0].Position = positions[0];
                    recycleTunnels[1].Position = positions[1];
                    Tunnels.Add(recycleTunnels[0]);
                    Tunnels.Add(recycleTunnels[1]);
                    recycleTunnels.RemoveRange(0, 2);
                }
            }            
   
            UpdateCamera();          
        }

        private void UpdateCamera()
        {
           if(front)
           {
               //move along the bird
               gameCam.Position.Z = flappyflappy.Position.Z + 2; 
               gameCam.Position.Y =flappyflappy.Position.Y+ 0.5f;
           }
           else
           {
               gameCam.Position.Z = flappyflappy.Position.Z;
           }
        }
        protected override void Render()
        {
            base.Render();

            Matrix4 view = gameCam.GetViewMatrix();
            AvailableShaders.ShaderPhong.view = view;
            AvailableShaders.ShaderPhong.lightDirection = Vector3.Normalize(new Vector3(1, -0.25f, -1)); // basic light
            
            //AxisGizmo.Render(Screen, Matrix4.CreateTranslation(viewTrans.X, viewTrans.Y - 0.5f, viewTrans.Z-3), view, projection);
            DrawEntities();            
        }

        private void DrawEntities()
        {
            flappyflappy.Render(Screen);
            foreach (Entity entity in Tunnels)
            {
                entity.Render(Screen);
            }
            foreach (Entity entity in passedTunnles)
            {
                entity.Render(Screen);
            }
        }
        
        private bool stateChange = false;
        private bool front = true;
        private void ChangeStateOfCamara()
        {
            if (!front)
            {
                if (stateChange)
                {                    
                    gameCam.Rotation.Y += (float)Time.DeltaTime * 200f;
                    gameCam.Position.X += 0.05f;
                    gameCam.Position.Z -= 0.15f;
                    if (gameCam.Rotation.Y >= 90 && gameCam.Position.X >= 5 && gameCam.Position.Z <= flappyflappy.Position.Z)
                    {
                         gameCam.Rotation.Y = 90;
                        gameCam.Position.X = 5;
                        gameCam.Position.Z = flappyflappy.Position.Z;
                        stateChange = !stateChange;
                    }
                    else
                    {
                        if (gameCam.Rotation.Y >= 90)
                        {
                            gameCam.Rotation.Y = 90;
                        }
                        if (gameCam.Position.X >= 5)
                        {
                            gameCam.Position.X = 5;
                        }
                        if (gameCam.Position.Z <= flappyflappy.Position.Z)
                        {
                            gameCam.Position.Z = flappyflappy.Position.Z;
                        }
                    }

                }
            }
            else
            {
                if (stateChange)
                {

                    gameCam.Rotation.Y -= (float)Time.DeltaTime * 200.0f;
                    gameCam.Position.X -= 0.05f;
                    gameCam.Position.Z += 0.15f;
                    if (gameCam.Rotation.Y <= 0 && gameCam.Position.X <= 0 && gameCam.Position.Z >= flappyflappy.Position.Z + 2)
                    {
                        gameCam.Rotation.Y = 0;
                        gameCam.Position.X = 0;
                        gameCam.Position.Z = flappyflappy.Position.Z + 2;
                        stateChange = !stateChange;
                    }
                    else
                    {
                        if (gameCam.Rotation.Y <= 0)
                        {
                            gameCam.Rotation.Y = 0;
                        }
                        if (gameCam.Position.X <= 0)
                        {
                            gameCam.Position.X = 0;
                        }
                        if ( gameCam.Position.Z >= flappyflappy.Position.Z + 2)
                        {
                            gameCam.Position.Z = flappyflappy.Position.Z + 2;
                        }
                    }
                }
            }
        }

        public List<Entity> GetTunnles()
        {
            return this.Tunnels;
        }       

        public void GameOver()
        {
            Init();            
        }       

        public void PassedTunnles(Entity[] tunnles)
        {
            foreach (Entity tunnel in tunnles)
            {               
                Tunnels.Remove(tunnel);
                passedTunnles.Add(tunnel);
            }
        }
        private List<Entity> recycleTunnels = new List<Entity>();

        public void AddPoints()
        {
            points++;
        }
        private int points;

        Random rand;
        private Vector3[] GetRandomPoint(float z)
        {          
            float centerY = rand.Next(-7, 7) / 10.0f;
            int maxRadius = (int)((1.2 - Math.Abs(centerY)) * 10);
            float radius = rand.Next(5 , maxRadius) / 10.0f;

            return new [] {new Vector3(0, centerY + radius, z) ,new Vector3(0, centerY - radius, z) } ;
        }

        private void EntityDestroyed(Entity entity)
        {
            recycleTunnels.Add(entity);
        }
    }
}
