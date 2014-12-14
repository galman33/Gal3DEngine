using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class TestGame : Game
    {

        private List<Model> models = new List<Model>();

        Random rand = new Random();

        private Camera cam;

        public TestGame() : base(640, 480)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cam = new Camera();
            for (int i = 0; i < 2; i++)
            {
                models.Add(new Model("Resources/Cat2.obj", "Resources/Cat2.png"));
            }
        }

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            cam.Position.Z += e.Delta * 0.1f;
        }

        protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Mouse.MiddleButton == OpenTK.Input.ButtonState.Pressed)
            {
                cam.Position.X += e.XDelta * 0.01f;
                cam.Position.Y += -e.YDelta * 0.01f;
            }
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == OpenTK.Input.Key.Space)
            {
                vel = 0.05f;
            }
        }

        private float vel =0 ;

        protected override void Update()
        {
            base.Update();
        }

        protected override void Render()
        {
            base.Render();

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 10.0f);
            Matrix4 view = cam.GetViewMatrix();
            
            for (int i = 0; i < 1; i++)
            {
                Matrix4 world = Matrix4.CreateRotationY((float) Time.TotalTime * 3) * Matrix4.CreateTranslation(0, 0, -3);

                models[i].Render(Screen, world, view, projection);
            }
        }

    }
}
