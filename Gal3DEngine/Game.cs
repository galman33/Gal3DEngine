using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Gal3DEngine
{
    public class Game : GameWindow
    {

        public int GameWidth { get; set; }
        public int GameHeight { get; set; }

        protected Screen Screen { get; set; }

        protected Color3 BackgroundColor { get; set; }

        public Game(int width, int height)
            : base()
        {
            GameWidth = width;
            GameHeight = height;

            Width = GameWidth;
            Height = GameHeight;

            VSync = VSyncMode.Off;

            WindowBorder = OpenTK.WindowBorder.Fixed;
            Screen = new Screen();

            Time.Init();

            Run(60, 60);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Screen.Init(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        int ticks = 0;
        int lastSec = 0;

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Time.Update();
            Update();

            //FPS
            ticks++;
            if (DateTime.Now.Second != lastSec)
            {
                Console.WriteLine(ticks + " FPS");

                ticks = 0;
                lastSec = DateTime.Now.Second;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);           

            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            Screen.Clear(BackgroundColor);

            Render();

            Screen.Render();
            SwapBuffers();
        }

        protected virtual void Render()
        {

        }

        protected virtual void Update()
        {

        }
        
    }
}
