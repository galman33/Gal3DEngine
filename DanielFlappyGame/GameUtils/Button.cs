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
    public class Button
    {
        public event ButtonPressed buttonPressed;
        private string text;
        private int width , height;
        private Vector2 location;

        private Color3[,] backGround;

        private Rectangle buttonRec;

        public Button(int width  , int height , Vector2 location, string text = "" , string backGroundPath = "")
        {
            this.width = width;
            this.height = height;
            this.location = location;
            buttonRec = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.text = text;
            LoadImage(backGroundPath);
            (Program.world).MouseUp += Button_MouseUp;
            //(Program.world).Mouse.ButtonDown += Mouse_ButtonDown;
        }

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

        private void LoadImage(string path)
        {
            if(path!= string.Empty)
            backGround = Texture.LoadTexture(path);
            else backGround = null;
        }      

        public void Render(Screen screen , TextRender render)
        {
            if(backGround!= null)
            ToolsHelper.RenderRectangle(screen, backGround, location);
            if (text != String.Empty)
            render.RenderText(screen, this.text, this.location + new Vector2(0f, this.height/6));            
        }
    }
}
