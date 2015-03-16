using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    public class Label
    {
        public string text;
       
        public Vector2 loc;

        public Label(string text ,Vector2 loc)
        {
            this.text = text;           
            this.loc = loc;
        }

        public void RenderLabel(Screen screen  , TextRender render)
        {
            render.RenderText(screen , this.text , this.loc);
        }
    }
}
