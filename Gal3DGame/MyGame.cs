using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gal3DEngine;
using OpenTK;
using Gal3DEngine.Gizmos;

namespace Gal3DGame
{
    class MyGame : Game
    {

        private Aircraft aircraft;
        private Camera camera;
        private Matrix4 projection;

        public MyGame() : base(640, 480)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Aircraft.LoadContent();

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 10.0f);

            camera = new Camera();
            camera.Position.Z = 4;

            aircraft = new Aircraft();
        }

        protected override void Render()
        {
            base.Render();

            Matrix4 view = camera.GetViewMatrix();

            AxisGizmo.Render(Screen, Matrix4.Identity, view, projection);

            aircraft.Render(Screen, projection, view);
        }

    }
}
