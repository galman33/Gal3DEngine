using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Gal3DEngine
{
	/// <summary>
	/// The main Game class responsible for passing the core game events such as Update and Render.
	/// New games should inherit from this class.
	/// </summary>
    public class Game : GameWindow
    {
		/// <summary>
		/// The game's screen width.
		/// </summary>
        public int GameWidth { get; set; }
		/// <summary>
		/// The game's screen height.
		/// </summary>
        public int GameHeight { get; set; }

		/// <summary>
		/// A reference to the screen.
		/// </summary>
        protected Screen Screen { get; set; }

		/// <summary>
		/// The desired background color of a clean window.
		/// </summary>
        protected Color3 BackgroundColor { get; set; }

		/// <summary>
		/// Create a new Game in the desired size.
		/// </summary>
		/// <param name="width">The desired width of the game's window.</param>
		/// <param name="height">The desired width of the game's window.</param>
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

		/// <summary>
		/// Called when the game's window was resized.
		/// </summary>
		/// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Screen.Init(Width, Height);
        }

		/// <summary>
		/// Called when the game is first loaded.
		/// </summary>
		/// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        int ticks = 0;
        int lastSec = 0;

		/// <summary>
		/// Called on frame update.
		/// </summary>
		/// <param name="e"></param>
        protected sealed override void OnUpdateFrame(FrameEventArgs e)
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

		/// <summary>
		/// Called on frame render.
		/// </summary>
		/// <param name="e"></param>
        protected sealed override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);           

            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            Screen.Clear(BackgroundColor);

            Render();

            Screen.Render();
            SwapBuffers();
        }

		/// <summary>
		/// Render logic should go in here.
		/// </summary>
        protected virtual void Render()
        {

        }

		/// <summary>
		/// Update logic should go in here.
		/// </summary>
        protected virtual void Update()
        {

        }
        
    }
}
