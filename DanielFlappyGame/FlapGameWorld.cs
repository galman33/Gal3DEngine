using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Gizmos;
using OpenTK.Input;
using Gal3DEngine.UTILS;
using DanielFlappyGame.GameUtils;
using DanielFlappyGame.Entities;

namespace DanielFlappyGame
{
    public class FlapGameWorld : Game
    {
        public FlappyBird flappyflappy;
        public PipesManager pipesManager;
        public Floor floor;        
        
        private Camera gameCam;
        Matrix4 projection;           

        private HighScoresManager HSManager;
        private TextRender textRender;
        private Label pointsLbl;
        private Label topLbl;

        public ShaderFlat curShader;

        private bool start;

        public FlapGameWorld() : base(640, 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            HSManager = new HighScoresManager();
            Program.world = this;            
            curShader = AvailableShaders.ShaderFlat;
            textRender = new TextRender(Fonts.ARIAL, Fonts.ARIALFONTDATA);
            pipesManager = new PipesManager();
            FlappyBird.LoadModel();

            this.BackgroundColor = new Color3(0, 0, 0);
            Init();
        }

        private void Init()
        {
            points = 0;
            gameCam = new Camera();
            front = true;           
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.3f, 10.0f);
            start = false;
            flappyflappy = new FlappyBird(new Vector3(0, 0, -2), new Vector3(0,(float)Math.PI, 0), Vector3.Normalize(new Vector3(1, -0.25f, -1)));
            floor = new Floor(new Vector3(-14.5f, -3.5f, 0), Vector3.One , @"Resources/road_damaged_0049_01_s.jpg");
            pipesManager.Init();
            curShader.projection = projection;

            pointsLbl = new Label("Points: "+ points , new Vector2(30,30));
            topLbl = new Label("Top Score: " + HSManager.GetTopScore().points , new Vector2(30, 60));
                 
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

            if(e.Key == Key.Space)
            {
                start = true;
            }
        }

        public void AddScore(int score  , string name)
        {
            HSManager.AddScore(new Score() { date = DateTime.Now, name = name, points = score });
        }
        
        protected override void Update()
        {
            base.Update();
            if (start)
            {
                // update entities
                flappyflappy.Update();
                pipesManager.Update();
                ChangeStateOfCamara();
                UpdateFloor();
            }
            UpdateCamera();          
        }

        private void UpdateFloor()
        {
            if(floor.translation.Z >this.gameCam.Position.Z)
            {               
                floor.Update(Vector3.Subtract(flappyflappy.Position, Vector3.UnitZ * 3));
            }
        }       
        private void UpdateCamera()
        {          
           if(front)
           {               
               gameCam.Position.Z = flappyflappy.Position.Z + 2.3f;               
               gameCam.Position.Y = flappyflappy.Position.Y;
                           
           }
           else
           {
               if (!stateChange)
               {
                   gameCam.Position.Z = flappyflappy.Position.Z;
                   gameCam.Position.X = 5;
               }
           }
        }

        protected override void Render()
        {
            base.Render();

            Matrix4 view = gameCam.GetViewMatrix();
            curShader.view = view;
            pointsLbl.text = "Points: " + points;
            pointsLbl.RenderLabel(Screen, textRender);
            topLbl.RenderLabel(Screen, textRender);           
            
            DrawEntities();            
        }

        private void DrawEntities()
        {
            floor.Render(Screen , 2);
            flappyflappy.Render(Screen);
            pipesManager.RenderPipes(Screen);            
        }
        
        private bool stateChange = false;
        private bool front = false;        
        
        private void ChangeStateOfCamara()
        {
            if (!front)
            {
                if (stateChange)
                {                    
                    gameCam.Rotation = Quaternion.Slerp(Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.PiOver2), gameCam.Rotation, (float)Time.DeltaTime * 5.0f);
                    gameCam.Position.X += 0.2f;
                    gameCam.Position.Z -= 0.15f;
                    
                    if (gameCam.Position.X >= 5 && gameCam.Position.Z <= flappyflappy.Position.Z) //&& gameCam.Rotation.Y == (float)(Math.PI / 2))
                    {                        
                        gameCam.Position.X = 5;
                        
                        gameCam.Position.Z = flappyflappy.Position.Z;
                        stateChange = !stateChange;
                    }
                    else
                    {                       
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

                    gameCam.Rotation = Quaternion.Slerp(gameCam.Rotation, Quaternion.FromAxisAngle(Vector3.UnitY, 0), (float)Time.DeltaTime * 5.0f);
                    gameCam.Position.X -= 0.15f;
                    gameCam.Position.Z += 0.15f;
                    if (gameCam.Position.X <= 0 && gameCam.Position.Z >= flappyflappy.Position.Z + 2)
                    {
                        
                        gameCam.Position.X = 0;
                        gameCam.Position.Z = flappyflappy.Position.Z + 2;
                        stateChange = !stateChange;
                    }
                    else
                    {                        
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
        } //change from front to side and the opposite

        public List<Pipe> GetTunnles()
        {
            return this.pipesManager.GetTunnles();
        }       

        public void GameOver()
        {
            AddScore(this.points, "Flappy");
            Init();            
        }        

        public void AddPoints()
        {
            points++;
        }
        private int points;        

        public void PassedTunnels(Pipe[] pipes)
        {
            pipesManager.PassedTunnles(pipes);
        }
    }
}
