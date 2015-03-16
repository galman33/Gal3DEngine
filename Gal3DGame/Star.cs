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

        public Star(Enviroment environment, Aircraft aircraft)
        {
            this.environment = environment;
            this.aircraft = aircraft;

            Reset();
        }

        private void Reset()
        {
            do
            {
                Vector3 position = new Vector3( (float)MyGame.Random.NextDouble() * 13 + 1,
                                                (float)MyGame.Random.NextDouble() * 1.5f + 0.2f,
                                                (float)MyGame.Random.NextDouble() * 13 + 1);
                hitBox = new Box(0.1f, 0.1f, 0.1f, position);
            } while (environment.IsCollidingWith(hitBox));
        }

        public static void LoadContent()
        {
            starModel = new Model("Resources/Star.obj", "Resources/Star.bmp");
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

            hitBox.DrawCube(screen, Matrix4.Identity, shader.view, shader.projection);
        }

    }
}
