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
    /// <summary>
    /// The Game screen.
    /// </summary>
    public class FlapGameWorld : Game
    {
        /// <summary>
        /// The FlappyBird(player) of the game.
        /// </summary>
        public FlappyBird flappyflappy;
        /// <summary>
        /// The Pipe manufacturer.
        /// </summary>
        public PipesManager pipesManager;
        /// <summary>
        /// The floor of the game.
        /// </summary>
        public Floor floor;        
        
        /// <summary>
        /// The Camera of the game.
        /// </summary>
        private Camera gameCam;
        Matrix4 projection;           

        private HighScoresManager HSManager;
        private TextRender textRender;
        /// <summary>
        /// Shows the current score of the game.
        /// </summary>
        private Label pointsLbl;
        /// <summary>
        /// Shows the best highscore of the computer.
        /// </summary>
        private Label topLbl;
        /// <summary>
        /// The current shader used for rendering of the game.
        /// </summary>
        public ShaderFlat curShader;
        /// <summary>
        /// Holds the data if to start the game or nit.
        /// </summary>
        private bool start;

        //CubeMap map;
        /// <summary>
        /// The constructor of the game
        /// </summary>
        public FlapGameWorld() : base(640, 480)
        {

        }

        /// <summary>
        /// Responsibles for the content loading of the game.
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Responsible for initiallizing the logic data of the game.
        /// </summary>
        private void Init()
        {
            points = 0;
            gameCam = new Camera();
            front = true;           
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.3f, 10.0f);
            start = false;
            flappyflappy = new FlappyBird(new Vector3(0, 0, -2), new Vector3(0,(float)Math.PI, 0), Vector3.Normalize(new Vector3(1, -0.25f, -1)));
            floor = new Floor(new Vector3(-2f, -1.3f, 0), Vector3.One , @"Resources/road_damaged_0049_01_s.jpg");
            pipesManager.Init();
            curShader.projection = projection;
           
            pointsLbl = new Label("Points: "+ points , new Vector2(30,30));
            topLbl = new Label("Top Score: " + HSManager.GetTopScore().points , new Vector2(30, 60));
                 
        }
        /// <summary>
        /// Firs when a key is pressed.
        /// </summary>
        /// <param name="e"></param>
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
        /// <summary>
        /// Adds new HighScore.
        /// </summary>
        /// <param name="score">The hiighscore</param>
        /// <param name="name">The nickname of the player.</param>
        public void AddScore(int score  , string name)
        {
            HSManager.AddScore(new Score() { date = DateTime.Now, name = name, points = score });
        }
        /// <summary>
        /// Updates the game data every frame.
        /// </summary>
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

        private float floorOffset = 1.0f;
        /// <summary>
        /// Updates the floor according to the FlappyBird player.
        /// </summary>
        private void UpdateFloor()
        {
            if(floor.translation.Z - floorOffset >this.gameCam.Position.Z)
            {               
                floor.Update(flappyflappy.Position);
            }
        }       
        /// <summary>
        /// Updates the Camera according to the FlappyBird player.
        /// </summary>
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
        /// <summary>
        /// Renders all the objects of the game into the Screen.
        /// </summary>
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
        /// <summary>
        /// Drwas all the entities of the game into the Screen.
        /// </summary>
        private void DrawEntities()
        {
            floor.Render(Screen , 2);
            flappyflappy.Render(Screen);
            pipesManager.RenderPipes(Screen);            
        }
        /// <summary>
        /// Camera prespective is changing.
        /// </summary>
        private bool stateChange = false;
        /// <summary>
        /// The camera prespective.
        /// </summary>
        private bool front = false;        
        /// <summary>
        /// Change the camera prespective(if needed)
        /// </summary>
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

        /// <summary>
        /// Returns the Pipes objects if the game.
        /// </summary>
        /// <returns></returns>
        public List<Pipe> GetPipes()
        {
            return this.pipesManager.GetPipes();
        }       
        /// <summary>
        /// Activates the GameOver mode.
        /// </summary>
        public void GameOver()
        {
            AddScore(this.points, Gal3DEngine.Utils.InputBox.Show("Name:" , "New Score" , "FlappyNewbie"));
            Init();            
        }        
        /// <summary>
        /// Adds points to the player.
        /// </summary>
        public void AddPoints()
        {
            points++;
        }
        /// <summary>
        /// The current points of the player in the game.
        /// </summary>
        private int points;        

        public void PassedPipes(Pipe[] pipes)
        {
            pipesManager.PassedPipes(pipes);
        }
    }
}
