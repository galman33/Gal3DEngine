using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Gal3DEngine
{
    class TestGame : Game
    {

        private Model model;
        private float scale = 0.5f;
        private float rotY = 0;
        private float rotX = MathHelper.Pi;
        private float transX = 0;
        private float transY = 0.25f;

        Random rand = new Random();

        public TestGame() : base(640, 480)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            model = new Model("Resources/Cat2.obj", "Resources/Cat2.png");
        }

        protected override void OnMouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            scale += e.Delta * 0.01f;
        }

        protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Mouse.LeftButton == OpenTK.Input.ButtonState.Pressed)
            {
                rotY += -e.XDelta * 0.01f;
                rotX += e.YDelta * 0.01f;
            }
            if (e.Mouse.MiddleButton == OpenTK.Input.ButtonState.Pressed)
            {
                transX += -e.XDelta * 0.01f;
                transY += e.YDelta * 0.01f;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Render()
        {
            base.Render();

            int camX = 0, camY = 0, camZ = 0;
            Matrix4 view = (Matrix4.CreateTranslation(camX, camY, camZ)).Inverted();

            Matrix4 world = Matrix4.CreateScale(scale) * Matrix4.CreateRotationY(rotY) * Matrix4.CreateRotationX(rotX) * Matrix4.CreateTranslation(transX, transY, 1);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 100);

            model.Render(Screen, world, view, projection);
        }

    }
}
