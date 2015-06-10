using Gal3DEngine;
using Gal3DEngine.UTILS;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DanielFlappyGame.GameUtils
{
    public delegate void ButtonPressed();
    /// <summary>
    /// Responsible for the buttons logic.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Fires when the button is pressed.
        /// </summary>
        public event ButtonPressed buttonPressed;
        /// <summary>
        /// The possible text of the button.
        /// </summary>
        private string text;
        /// <summary>
        /// The width of the Button.
        /// </summary>
        private int width;
        /// <summary>
        /// The height of the Button.
        /// </summary>
        private int height;
        /// <summary>
        /// The location of the Button on the screen.
        /// </summary>
        private Vector2 location;

        /// <summary>
        /// The background (image) of the Button.
        /// </summary>
        private Color3[,] backGround;
        /// <summary>
        /// The rectangle represnting the Button bounds.
        /// </summary>
        private Rectangle buttonRec;
        /// <summary>
        /// Initiallizes a button from given rectangle(width, height and location), text and backgroundImage path.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="location"></param>
        /// <param name="text"></param>
        /// <param name="backGroundPath">The background image path.</param>
        public Button(int width  , int height , Vector2 location, string text = "" , string backGroundPath = "")
        {
            this.width = width;
            this.height = height;
            this.location = location;
            buttonRec = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.text = text;
            LoadImage(backGroundPath);
            (Program.world).MouseUp += Button_MouseUp;            
        }

        /// <summary>
        /// Fires the mouse is pressed inside the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                if (buttonRec.Contains(e.X, e.Y))
                {
                    if (buttonPressed != null)
                        buttonPressed();
                }
            }
        }
        /// <summary>
        /// Fires the mouse is pressed inside the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == MouseButton.Left)
            {
                if(buttonRec.Contains(e.X , e.Y))
                {
                    if (buttonPressed != null)
                        buttonPressed();
                }
            }
        }        
        /// <summary>
        /// Loads the backgrund image from given path.
        /// </summary>
        /// <param name="path">The path of the image.</param>
        private void LoadImage(string path)
        {
            if(path!= string.Empty)
            backGround = Texture.LoadTexture(path);
            else backGround = null;
        }      
        /// <summary>
        /// Renders the button into a given screen with a given TextRender.
        /// </summary>
        /// <param name="screen">The screen to render to.</param>
        /// <param name="render">The Text Render to render with.</param>
        public void Render(Screen screen , TextRender render)
        {
            if(backGround!= null)
            ToolsHelper.RenderRectangle(screen, backGround, location);
            if (text != String.Empty)
            render.RenderText(screen, this.text, this.location + new Vector2(0f, this.height/6));            
        }
    }
}
