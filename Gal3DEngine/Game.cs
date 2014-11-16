using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Gal3DEngine
{
    class Game : GameWindow
    {

        public int GameWidth = 640;
        public int GameHeight = 480;

        private Screen screen;

        private Model model;

        public Game()
            : base()
        {
            Width = GameWidth;
            Height = GameHeight;

            VSync = VSyncMode.Off;

            WindowBorder = OpenTK.WindowBorder.Fixed;
            screen = new Screen();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            screen.Init(Width, Height);
        }

        VertexUV[] vertices;
        int[] indices;
        Color3[,] texture;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //model = new Model("Resources/Sasuke.obj");

            vertices = new VertexUV[3];
            vertices[0].Position = new Vector4(-0.5f, 0.5f, -1, 1);
            vertices[0].UV = new Vector2(0, 0);
            vertices[1].Position = new Vector4(0.5f, 0.5f, -1, 1);
            vertices[1].UV = new Vector2(1, 0);
            vertices[2].Position = new Vector4(-0.5f, -0.5f, -1, 1);
            vertices[2].UV = new Vector2(0, 1);

            indices = new int[] { 0, 1, 2 };

            texture = Texture.LoadTexture("Resources/TesTexture.png");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        Random rand = new Random();

        int ticks = 0;
        int lastSec = 0;

        float f = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            f += 0.04f;

            GL.ClearColor(1.0f, 0.0f, 1.0f, 1.0f);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            screen.Clear(new Color3());

            int camX = 0, camY = 0, camZ = 0;
            float rot = 0;
            Matrix4 view = (Matrix4.CreateTranslation(camX, camY, camZ)).Inverted();

            Matrix4 world = Matrix4.CreateScale(0.005f) * Matrix4.CreateRotationY(f) * Matrix4.CreateRotationX(MathHelper.Pi) * Matrix4.CreateTranslation(0, 0.25f, 1);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 0.1f, 100);

            //model.Render(screen, world, view, projection);

            ShaderUV.projection = ShaderUV.view = ShaderUV.world = Matrix4.Identity;
            ShaderUV.texture = texture;
            ShaderUV.Render(screen, vertices, indices);

            screen.Render();

            SwapBuffers();

            ticks++;
            if (DateTime.Now.Second != lastSec)
            {
                Console.WriteLine(ticks + " FPS");

                ticks = 0;
                lastSec = DateTime.Now.Second;
            }
        }

        private void CreateCube(List<Vector3> vertices)
        {
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f)); // triangle 1 : begin
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f)); // triangle 1 : end
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f)); // triangle 2 : begin
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f)); // triangle 2 : end
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f,-1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(-1.0f, 1.0f, 1.0f));
            vertices.Add(new Vector3(1.0f,-1.0f, 1.0f));
        }

    }
}
