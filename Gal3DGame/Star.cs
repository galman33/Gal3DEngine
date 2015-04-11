using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.UTILS;

namespace Gal3DGame
{
    class Star
    {
        private static Model starModel;

        private static ShaderFlat shader = AvailableShaders.ShaderFlat;

        private Box hitBox;

        private Enviroment environment;
        private Aircraft aircraft;
        private MyGame game;

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

        public static void LoadContent()
        {
            starModel = new Model("Resources/Star.obj", "Resources/Star.bmp");
        }

        public void Update()
        {
            if (Box.IsColliding(hitBox, aircraft.CollisionBox))
            {
                game.AddPoint();
                Reset();
            }
        }

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
