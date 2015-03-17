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
    public class MenuWorld : Game
    {
        TextRender textRender;
        Button tmpBtn;
        public MenuWorld() : base(640 , 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            (Program.world) = this;
            textRender = new TextRender(Fonts.ARIAL, Fonts.ARIALFONTDATA);
            tmpBtn = new Button(100, 50, new Vector2(40, 40), "Start", "Resources\\btnBackGround.png");
            tmpBtn.buttonPressed += OnBtnClick;
        }

        protected override void Render()
        {
            base.Render();
            textRender.RenderText(Screen, "To Start Press Space", new Vector2(30, 30));
            tmpBtn.Render(Screen, textRender);
        }

        private void OnBtnClick()
        {
            StartFlapGame();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if( e.Key == OpenTK.Input.Key.Space)
            {
                StartFlapGame();
            }
        }

        private void StartFlapGame()
        {
            Program.world = new FlapGameWorld();
            Close();
        }
    }
}
