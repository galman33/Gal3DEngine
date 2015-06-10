using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Utils;

namespace Gal3DGame
{
	/// <summary>
	/// An in-game collectable start element.
	/// </summary>
    class Star
    {
        private static Model starModel;

        private static ShaderFlat shader = AvailableShaders.ShaderFlat;

        private Box hitBox;

        private Enviroment environment;
        private Aircraft aircraft;
        private MyGame game;

		/// <summary>
		/// Create a new star element.
		/// </summary>
		/// <param name="environment">A reference to the environment.</param>
		/// <param name="aircraft">A reference to the aircraft.</param>
		/// <param name="game">A reference to the game manager</param>
        public Star(Enviroment environment, Aircraft aircraft, MyGame game)
        {
            this.environment = environment;
            this.aircraft = aircraft;
            this.game = game;

            Reset();
        }

        private void Reset()
        {
            int x, y;
            environment.GetFreeTile(out x, out y);
            Vector3 position = new Vector3( x + 0.5f,
                                            (float)RandomHelper.Random.NextDouble() * 1.5f + 0.2f,
                                            y + 0.5f);
            hitBox = new Box(0.1f, 0.1f, 0.1f, position);
        }

		/// <summary>
		/// Load into memory the resources used by the star.
		/// </summary>
        public static void LoadContent()
        {
            starModel = new Model("Resources/Star.obj", "Resources/Star.bmp");
        }

		/// <summary>
		/// Update the star.
		/// </summary>
        public void Update()
        {
            if (Box.IsColliding(hitBox, aircraft.CollisionBox))
            {
                game.AddPoint();
                Reset();
            }
        }

		/// <summary>
		/// Render the star.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <param name="projection">The projection Matrix.</param>
		/// <param name="view">The view matrix.</param>
        public void Render(Screen screen, Matrix4 projection, Matrix4 view)
        {
            shader.world =  Matrix4.CreateRotationZ((float)Time.TotalTime * 2.0f) * //Rotate
                            Matrix4.CreateRotationX(MathHelper.PiOver2) *
                            Matrix4.CreateScale(0.05f) *
                            Matrix4.CreateTranslation(hitBox.origin);
            shader.view = view;
            shader.projection = projection;

            shader.lightDirection = (new Vector3(-1, -1, -1)).Normalized();
            shader.ambientLight = 0.3f;

            shader.ExtractData(starModel);

            shader.Render(screen);

            //hitBox.DrawCube(screen, Matrix4.Identity, shader.view, shader.projection);
        }

    }
}
