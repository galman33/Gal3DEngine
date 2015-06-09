using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    /// <summary>
    /// A util responsible for rendering a label into the Screen.
    /// </summary>
    public class Label
    {
        /// <summary>
        /// The text of the label.
        /// </summary>
        public string text;
       /// <summary>
       /// The position of the label.
       /// </summary>
        public Vector2 loc;

        /// <summary>
        /// Initiallize a label with given text and position.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="loc"></param>
        public Label(string text ,Vector2 loc)
        {
            this.text = text;           
            this.loc = loc;
        }
        /// <summary>
        /// Renders the Label to a given Screen with given TextRender
        /// </summary>
        /// <param name="screen">The screen to render to.</param>
        /// <param name="render">The text render to render with.</param>
        public void RenderLabel(Screen screen  , TextRender render)
        {
            render.RenderText(screen , this.text , this.loc);
        }
    }
}
