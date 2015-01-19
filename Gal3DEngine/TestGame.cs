using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Gal3DEngine.Gizmos;
using Gal3DEngine.IndicesTypes;

namespace Gal3DEngine
{
    class TestGame : Game
    {

        private List<Model> models = new List<Model>();

        Random rand = new Random();

        Color3[,] tex;

        private Camera cam;

        public TestGame() : base(640, 480)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cam = new Camera();
            cam.Position.Z = 3;
            tex = Texture.LoadTexture("Resources/Cat2.png");
            /*for (int i = 0; i < 2; i++)
            {
                models.Add(new Model("Resources/Cat2.obj", "Resources/Cat2.png"));
            }*/
            SetUp();
        }

        Vector4[] positions;
        Vector2[] uvs;
        Vector3[] Normal;
        IndexPositionUVNormal[] indices;

        private void SetUp()
        {
            positions = new Vector4[4];
            positions[0] = new Vector4(1, 1, 0, 1);
            positions[1] = new Vector4(1, 0, 0, 1);
            positions[2] = new Vector4(0, 0, 0, 1);
            positions[3] = new Vector4(0, 1, 0, 1);

            uvs = new Vector2[4];
            uvs[0] = new Vector2(1, 1);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 0);
            uvs[3] = new Vector2(0, 1);

            Normal = new Vector3[] { new Vector3(0, 0, 1) };

            indices = new IndexPositionUVNormal[6];
            indices[0] = new IndexPositionUVNormal(0, 0, 0);
            indices[1] = new IndexPositionUVNormal(3, 3, 0);
            indices[2] = new IndexPositionUVNormal(2, 2, 0);

            indices[3] = new IndexPositionUVNormal(1, 1, 0);
            indices[4] = new IndexPositionUVNormal(0, 0, 0);
            indices[5] = new IndexPositionUVNormal(2, 2, 0);
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

            //DrawModel(projection, view);

            //AxisGizmo.Render(Screen, Matrix4.Identity, view, projection);

            AvailableShaders.ShaderFlat.projection = projection;
            AvailableShaders.ShaderFlat.view = view;
            AvailableShaders.ShaderFlat.lightDirection = Vector3.Normalize(new Vector3(0, 0, -1));

            AvailableShaders.ShaderFlat.texture = tex;
            AvailableShaders.ShaderFlat.world = Matrix4.CreateRotationY((float) Math.Sin(Time.TotalTime * 3));
            //AvailableShaders.ShaderFlat.world = Matrix4.Identity;
            AvailableShaders.ShaderFlat.SetVerticesPositions(positions);
            AvailableShaders.ShaderFlat.SetVerticesUvs(uvs);
            AvailableShaders.ShaderFlat.SetVerticesNormals(Normal);
            AvailableShaders.ShaderFlat.SetIndices(indices);
            AvailableShaders.ShaderFlat.Render(Screen);
        }

        private void DrawModel(Matrix4 projection, Matrix4 view)
        {
            AvailableShaders.ShaderFlat.projection = projection;
            AvailableShaders.ShaderFlat.view = view;

            AvailableShaders.ShaderFlat.lightDirection = Vector3.Normalize(new Vector3(0, 0, -1));

            for (int i = 0; i < 1; i++)
            {
                AvailableShaders.ShaderFlat.world = Matrix4.CreateRotationY((float)Time.TotalTime * 3) * Matrix4.CreateTranslation(0, 0, -3);
                AvailableShaders.ShaderFlat.ExtractData(models[i]);
                AvailableShaders.ShaderFlat.Render(Screen);

                AxisGizmo.Render(Screen, AvailableShaders.ShaderFlat.world, view, projection);
            }
        }

    }
}
