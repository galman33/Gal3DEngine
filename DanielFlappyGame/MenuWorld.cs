using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
namespace DanielFlappyGame
{
    public class MenuWorld : Game
    {
        public MenuWorld() : base(640 , 480)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
        }

        protected override void Render()
        {
            base.Render();
            TextRender.BasicDrawText(Screen, "To Start Press Space", Fonts.ARIAL, Fonts.ARIALFONTDATA, new Vector2(30, 30));
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if( e.Key == OpenTK.Input.Key.Space)
            {
                Program.world = new FlapGameWorld();
                Close();
            }
        }
    }
}
