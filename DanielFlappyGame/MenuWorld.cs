using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using DanielFlappyGame.GameUtils;
namespace DanielFlappyGame
{
    /// <summary>
    /// The first screen of the game.
    /// </summary>
    public class MenuWorld : Game
    {
        TextRender textRender;
        Button tmpBtn;
        Button highScoreBtn;

        HighScoresManager manager;
        
        bool showHighScores;

        List<Score> scores;
        /// <summary>
        /// Initiallize the Menu world.
        /// </summary>
        public MenuWorld() : base(640 , 480)
        {

        }
        /// <summary>
        /// Load the content regarding the Menu world.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            (Program.world) = this;
            manager = new HighScoresManager();
            textRender = new TextRender(Fonts.ARIAL, Fonts.ARIALFONTDATA);            
            highScoreBtn = new Button(30, 30, new Vector2(30, 60), "", "Resources\\HighScoresImg.png");            
            highScoreBtn.buttonPressed += highScoreBtn_buttonPressed;

            scores = manager.GetScores();
            if (scores.Count > 5)
                scores.RemoveRange(0, scores.Count - 5);
            scores.Reverse();
            
        }
        /// <summary>
        /// Shows or hides the HighScores of the computer.
        /// </summary>
        void highScoreBtn_buttonPressed()
        {
            showHighScores = !showHighScores;
        }
        /// <summary>
        /// Renders the Menu world screen.
        /// </summary>
        protected override void Render()
        {
            base.Render();
            textRender.RenderText(Screen, "To Start Press Space", new Vector2(30, 30));
            highScoreBtn.Render(Screen, textRender);
            //tmpBtn.Render(Screen, textRender);
            if (showHighScores)
            {
                RenderHighScore();
            }
            
        }

        /// <summary>
        /// Starts the Flap Game.
        /// </summary>
        private void OnBtnClick()
        {
            StartFlapGame();
        }
        /// <summary>
        /// Renders the highScores of the computer.
        /// </summary>
        private void RenderHighScore()
        {            
            for(int i= 0; i< scores.Count; i++)
            {
                Label lable = new Label(scores[i].ToString(), new Vector2(30, 30 * (i + 4)));
                lable.RenderLabel(Screen, textRender);
            }
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if( e.Key == OpenTK.Input.Key.Space)
            {
                StartFlapGame();
            }
        }
       
        /// <summary>
        /// Change thw world to the Flap Game world (the actual game).
        /// </summary>
        private void StartFlapGame()
        {
            Program.world = new FlapGameWorld();
            Close();
        }
    }
}
