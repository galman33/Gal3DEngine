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
    public class FlapGameWorld : Game
    {
        private List<Pipe> Tunnels = new List<Pipe>();
        private List<Pipe> passedTunnles = new List<Pipe>(); // the bird already passed them but the camera still sees them
        public FlappyBird flappyflappy;
        public Floor floor;

        private TimeSpan delta = new TimeSpan(0, 0, 0, 1, 500);
        private TimeSpan curDelta = TimeSpan.Zero;
        
        private Camera gameCam;
        Matrix4 projection;           

        private HighScoresManager HSManager;

        public ShaderFlat curShader;

        public FlapGameWorld() : base(640, 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
           
            base.OnLoad(e);
            HSManager = new HighScoresManager();
            Program.world = this;
            rand = new Random();
            curShader = AvailableShaders.ShaderFlat;

            Pipe.LoadModel();
            FlappyBird.LoadModel();

            this.BackgroundColor = new Color3(0, 0, 0);
            Init();
        }

        private void Init()
        {
            points = 0;
            gameCam = new Camera();
            
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.3f, 10.0f);

            flappyflappy = new FlappyBird(new Vector3(0, 0, -2), Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1)));
            floor = new Floor(new Vector3(-1.5f, -1.5f, 0), Vector3.One , @"Resources/road_damaged_0049_01_s.jpg");

            //clear all tunnles in the game for game to start
            Tunnels.Clear();
            passedTunnles.Clear();

            Vector3[] positions = GetRandomPoint(-3f + flappyflappy.Position.Z);
            Tunnels.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Tunnels.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
            Tunnels[Tunnels.Count - 1].DestroyEntity += EntityDestroyed;
            Tunnels[Tunnels.Count - 2].DestroyEntity += EntityDestroyed;

            curShader.projection = projection;
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

        public void AddScore(int score  , string name)
        {
            HSManager.AddScore(new Score() { date = DateTime.Now, name = name, points = score });
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

                Vector3[] positions = GetRandomPoint(-3f + flappyflappy.Position.Z);
                if (recycleTunnels.Count ==0)
                {
                    Tunnels.Add(new Pipe(positions[0], new Vector3(0, 0, (float)Math.PI), Vector3.Normalize(new Vector3(1, -0.25f, -1))));
                    Tunnels.Add(new Pipe(positions[1], Vector3.Zero, Vector3.Normalize(new Vector3(1, -0.25f, -1))));
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
            UpdateFloor();
            UpdateCamera();          
        }

        private void UpdateFloor()
        {
            if(floor.translation.Z >this.gameCam.Position.Z)
            {                
                floor.translation.Z = gameCam.Position.Z-3;
            }
        }

        private void UpdateCamera()
        {
           
           if(front)
           {
               //move along the bird
               gameCam.Position.Z = flappyflappy.Position.Z + 2.3f; 
              // if(Math.Abs(gameCam.Position.Y - flappyflappy.Position.Y) > 0.8)
               //{
                   gameCam.Position.Y = flappyflappy.Position.Y;
                   //gameCam.Position.Y += (gameCam.Position.Y > flappyflappy.Position.Y) ? -0.15f : +0.15f;
              // }               
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
            curShader.view = view;            
            TextRender.BasicDrawText(Screen, "Points: " + points, Fonts.ARIAL, Fonts.ARIALFONTDATA, new Vector2(30, 30));
            TextRender.BasicDrawText(Screen , "Top: "+ HSManager.GetTopScore().points ,Fonts.ARIAL, Fonts.ARIALFONTDATA, new Vector2(30, 60));
            //AxisGizmo.Render(Screen, Matrix4.CreateTranslation(viewTrans.X, viewTrans.Y - 0.5f, viewTrans.Z-3), view, projection);
            DrawEntities();            
        }

        private void DrawEntities()
        {
            floor.Render(Screen , 2);
            flappyflappy.Render(Screen);
            foreach (Pipe entity in Tunnels)
            {
                entity.Render(Screen);
            }
            foreach (Pipe entity in passedTunnles)
            {
                entity.Render(Screen);
            }
            
        }
        
        private bool stateChange = false;
        private bool front = true;
        private float t = 0.0f;
        
        private void ChangeStateOfCamara()
        {
            if (!front)
            {
                if (stateChange)
                {
                     t += (float)(Time.DeltaTime)*0.4f;
                    gameCam.Rotation = Quaternion.Slerp(Quaternion.Identity, Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.PiOver2), t);
                    gameCam.Position.X += 0.2f;
                    gameCam.Position.Z -= 0.15f;
                    
                    if (gameCam.Position.X >= 5 && gameCam.Position.Z <= flappyflappy.Position.Z && t <=1) //&& gameCam.Rotation.Y == (float)(Math.PI / 2))
                    {                        
                        gameCam.Position.X = 5;
                        t = 0.99f;
                        gameCam.Position.Z = flappyflappy.Position.Z;
                        stateChange = !stateChange;
                    }
                    else
                    {
                        if (t <= 1)
                        {
                            t = 1f;
                            gameCam.Rotation = Quaternion.Slerp(Quaternion.Identity, Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.PiOver2), t);
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
                /*if (stateChange)
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
                }*/
            }
        }

        public List<Pipe> GetTunnles()
        {
            return this.Tunnels;
        }       

        public void GameOver()
        {
            AddScore(this.points, "Flappy");
            Init();            
        }

        public void PassedTunnles(Pipe[] tunnles)
        {
            foreach (Pipe tunnel in tunnles)
            {               
                Tunnels.Remove(tunnel);
                passedTunnles.Add(tunnel);
            }
        }
        private List<Pipe> recycleTunnels = new List<Pipe>();

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
            recycleTunnels.Add((entity as Pipe));
        }
    }
}
